using Microsoft.AspNetCore.Mvc;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;

namespace QuizApp_API.Controllers
{
    [Route("api/quizzes")]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizzesService _service;

        public QuizzesController(IQuizzesService service)
        {
            _service = service;
        }

        // GET: api/Quizzes/FullList
        [HttpGet("full-list")]
        public async Task<IEnumerable<QuizModel>> GetQuiz(int? startIndex, int? endIndex)
        {
            if (startIndex == null || endIndex == null)
            {
                return await _service.GetAsync();
            }
            if (startIndex == 0 && startIndex == endIndex)
            {
                return await _service.GetAsync();
            }
            return new List<QuizModel>();
        }

        // GET: api/Quizzes/List
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

        // GET: api/Quizzes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<QuizModel>> GetQuiz(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return await _service.GetQuizAsync(id);
            }
            return NotFound();
        }

        [HttpPost("create")]
        public async Task<ActionResult> CreateQuiz([FromBody] QuizModel quiz)
        {
            if (quiz != null)
                await _service.AddQuizAsync(quiz);
            return Ok();
        }
    }
}
