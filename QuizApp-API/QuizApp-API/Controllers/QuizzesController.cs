using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.Controllers
{
    [Route("api/quizzes")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizzesService _service;
        private readonly IQuizResultsService _results;

        public QuizzesController(IQuizzesService service, IQuizResultsService results)
        {
            _service = service;
            _results = results;
        }

        [HttpGet("list")]
        public async Task<IEnumerable<QuizListItemModel>> GetQuizList(int? startIndex, int? endIndex)
        {
            if (startIndex == null || endIndex == null)
            {
                return await _service.GetTitlesAsync();
            }
            if (startIndex == 0 && startIndex == endIndex)
            {
                return await _service.GetTitlesAsync();
            }
            return new List<QuizListItemModel>();
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
                if (string.IsNullOrEmpty(value))
                    return await _service.GetTitlesAsync();
                else
                    return await _service.SearchAsync(value);
            } 
            catch(Exception ex)
            {
                // TODO: Log this error
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
                await _results.SaveResultAsync(result);
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
                if (quiz.AuthorId == GetCurrentUserId())
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
                return User.Claims.FirstOrDefault(c => c.Type == "UserId").Value;
            else
                return string.Empty;
        }
    }
}
