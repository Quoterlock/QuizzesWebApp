using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.BusinessLogic.Services;

namespace QuizApp_API.Controllers
{
    [Route("api/quizzes")]
    public class QuizzesController(IQuizzesService service,
        IQuizResultsService results,
        RedisService cache) 
        : ControllerBase
    {
        private readonly RedisService _cache = cache;
        private readonly IQuizzesService _service = service;
        private readonly IQuizResultsService _results = results;

        [HttpGet("list")]
        public async Task<IEnumerable<QuizListItemModel>> GetQuizList(int? startIndex, int? endIndex)
        {
            // check cache
            try
            {
                var isCacheAvailable = _cache.CheckConnection();
                var key = $"quiz-list-{startIndex}-{endIndex}";
                if (isCacheAvailable && await _cache.IsExistsAsync(key))
                {
                    var list = await _cache.GetAsync<List<QuizListItemModel>>(key);
                    return list ?? [];
                }
                // get list items
                var titles = await _service.GetTitlesAsync() ?? [];
                if (isCacheAvailable)
                    await _cache.SetAsync(key, titles);
                return titles;
            } catch (Exception ex)
            {
                return [];
            }
            
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<QuizModel>> GetQuiz(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    var quiz = await _service.GetByIdAsync(id);
                    return quiz;                    
                } 
                catch(Exception ex)
                {
                    return NotFound(ex.Message);
                }
            }
            return NotFound();
        }

        [HttpGet("search")]
        public async Task<IEnumerable<QuizListItemModel>> SearchQuizzes(string value)
        {
            try
            {
                List<QuizListItemModel> list;
                var key = $"quiz-search-{value}";
                if (!await _cache.IsExistsAsync(key))
                {
                    if (string.IsNullOrEmpty(value))
                        list = await _service.GetTitlesAsync();
                    else
                        list = await _service.SearchAsync(value);

                    await _cache.SetAsync(key, list ?? []);
                    return list ?? [];
                }
                else
                {
                    list = (await _cache.GetAsync<List<QuizListItemModel>>(key)) ?? [];
                    return list;
                }
            } 
            catch(Exception ex)
            {
                return [];
            }
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<ActionResult> CreateQuiz([FromBody] QuizModel quiz)
        {
            if (quiz == null)
                return BadRequest();
            try
            {
                quiz.Author.Owner.Id = GetCurrentUserId();
                await _service.AddQuizAsync(quiz);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("save-result")]
        [Authorize]
        public async Task<ActionResult> SaveResult([FromBody] QuizResultModel result)
        {
            try
            {
                var userId = GetCurrentUserId();
                await _results.SaveResultAsync(result.QuizId, userId, result.Result);
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("delete/{id}")]
        [Authorize]
        public async Task<ActionResult> DeleteQuiz(string id)
        {
            try
            {
                var quiz = await _service.GetByIdAsync(id);
                if (quiz.Author.Owner.Id == GetCurrentUserId())
                {
                    await _service.RemoveQuizAsync(id);
                    return Ok();
                }
                else return BadRequest("You are not an author!");
            } 
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GetCurrentUserId()
        {
            if (User.Claims != null && User.Claims.Any(c => c.Type == "UserId"))
            {
                var userClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if(userClaim != null)
                    return userClaim.Value;
                return string.Empty;
            }
            else return string.Empty;
        }
    }
}
