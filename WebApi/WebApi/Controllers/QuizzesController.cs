using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using WebApi.BusinessLogic.Interfaces;
using WebApi.BusinessLogic.Models;
using WebApi.DataAccess.Entities;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuizzesController : ControllerBase
    {
        private readonly IQuizzesService _service;

        public QuizzesController(IQuizzesService service)
        {
            _service = service;
        }

        // GET: api/Quizzes/FullList
        [HttpGet("FullList")]
        public async Task<List<QuizModel>> GetQuiz(int? startIndex, int? endIndex)
        {
            if (startIndex == null || endIndex == null) 
            {
                return await _service.GetAsync();
            }
            if(startIndex == 0 && startIndex == endIndex)
            {
                return await _service.GetAsync();
            }
            return new List<QuizModel>();
        }

        // GET: api/Quizzes/List
        [HttpGet("List")]
        public async Task<List<QuizListItemModel>> GetQuizList(int? startIndex, int? endIndex)
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
            if(!string.IsNullOrEmpty(id))
            {
                return await _service.GetQuizAsync(id);
            }
            return NotFound();
        }

        [HttpPost("CreateQuiz")]
        public async Task<ActionResult> CreateQuiz([FromBody] QuizModel quiz)
        {
            if(quiz != null)
                await _service.AddQuizAsync(quiz);
            return Ok();
        }
    }
}
