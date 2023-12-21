using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Interfaces;
using WebApi.BusinessLogic.Models;
using WebApi.DataAccess.Data;
using WebApi.DataAccess.Entities;
using WebApi.DataAccess.Interfaces;

namespace WebApi.BusinessLogic
{
    public class QuizzesService : IQuizzesService
    {
        private readonly IQuizzesRepository _repository;
        public QuizzesService(IQuizzesRepository repository)
        { 
            _repository = repository;
        }
        public async Task<List<QuizModel>> GetAsync()
        {
            var models = new List<QuizModel>();
            var entities = await _repository.AsyncGet();
            foreach (var entity in entities)
                models.Add(Convert(entity));
            return models;
        }

        public async Task<List<QuizModel>> GetAsync(int from, int to)
        {
            var models = new List<QuizModel>();
            var entities = await _repository.AsyncGet(from, to);
            foreach (var entity in entities)
                models.Add(Convert(entity));
            return models;
        }

        public async Task<QuizModel> GetQuizAsync(string id)
        {
            var entity = await _repository.AsyncGet(id);
            return entity != null ? Convert(entity) : new QuizModel();
        }

        public Task<List<QuizListItemModel>> GetTitlesAsync(int from, int to)
        {
            throw new NotImplementedException();
        }

        public Task<List<QuizListItemModel>> GetTitlesAsync()
        {
            throw new NotImplementedException();
        }

        private QuizModel Convert(Quiz entity)
        {
            var model = new QuizModel { Id = entity.Id, Title = entity.Title };
            var questions = new List<QuestionModel>();
            foreach(Question question in entity.Questions)
            {
                var options = new List<OptionModel>();
                foreach (var option in question.Options)
                options.Add(new OptionModel { Text = option.Text ?? "none" });
                questions.Add(new QuestionModel{
                    Options = options,
                    CorrectAnswerIndex = question.CorrectAnswerIndex,
                    Text = question.Title ?? "none"
                });
            }
            model.Questions = questions.ToArray();
            return model;
        }
    }
}
