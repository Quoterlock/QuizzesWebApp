using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver.Linq;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic
{
    public class QuizzesService(
        IQuizzesRepository repository, 
        IQuizResultsService resultsService, 
        IRatesService ratesService) : IQuizzesService
    {
        private readonly IQuizzesRepository _repository = repository;
        private readonly IQuizResultsService _resultsService = resultsService;
        private readonly IRatesService _ratesService = ratesService;

        public async Task AddQuizAsync(QuizModel quiz)
        {
            ArgumentNullException.ThrowIfNull(quiz);

            if (string.IsNullOrEmpty(quiz.Title))
                throw new ArgumentException("Question title is null or empty");

            try
            {
                quiz.CreationDate = DateTime.Now.ToString();
                await _repository.AddAsync(Convert(quiz));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<QuizModel> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            var entity = await _repository.GetByIdAsync(id) 
                ?? throw new Exception("Entity doesn't exist with id:" + id);

            var model = Convert(entity);
            
            // get results
            model.Results = await _resultsService.GetResultsByQuizIdAsync(model.Id);
            
            // get rates
            var rates = await _ratesService.GetRatesAsync(model.Id);
            if (!rates.IsNullOrEmpty())
                model.Rate = rates.Average(e => e.Rate);

            return model;
        }

        public async Task<IEnumerable<QuizListItemModel>> GetTitlesAsync(int from, int to)
        {
            var list = await GetRangeAsync(from, to);
            return GetTitles(list);
        }

        public async Task<IEnumerable<QuizListItemModel>> GetTitlesAsync()
        {
            var list = await GetAllAsync();
            return GetTitles(list);
        }

        public async Task RemoveQuizAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));
            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<QuizListItemModel>> GetAllTitlesByAuthorId(string profileId)
        {
            // get quizzes
            var entities = await _repository.GetByAuthorAsync(profileId) ?? [];
            var models = ConvertEntitiesToModels(entities);

            // add rates
            models = await AddRatesForQuizzes(models);
            return GetTitles(models);
        }

        public async Task<IEnumerable<QuizListItemModel>> SearchAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));

            var entities = await _repository.SearchAsync(value);
            var models = ConvertEntitiesToModels(entities);
            return GetTitles(models);
        }

        private async Task<IEnumerable<QuizModel>> AddRatesForQuizzes(IEnumerable<QuizModel> quizzes)
        {
            var rates = await _ratesService.GetRatesAsync(
                quizzes.Select(e => e.Id).ToArray());
            foreach (var quiz in quizzes)
            {
                var quizRates = rates.Where(r => r.QuizId == quiz.Id);
                if (quizRates.IsNullOrEmpty())
                    quiz.Rate = 0;
                else
                    quiz.Rate = quizRates.Average(e => e.Rate);
            }
            return quizzes;
        }

        private async Task<IEnumerable<QuizModel>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync() ?? [];
            var models = ConvertEntitiesToModels(entities);
            return await AddRatesForQuizzes(models);
        }

        private async Task<IEnumerable<QuizModel>> GetRangeAsync(int from, int to)
        {
            if (from <= to)
            {
                var entities = await _repository.GetRangeAsync(from, to) ?? [];
                var models = ConvertEntitiesToModels(entities);
                return models;
            }
            else throw new Exception("\"From\" cannot be larger that \"to\"");
        }

        private static IEnumerable<QuizModel> ConvertEntitiesToModels(IEnumerable<Quiz> entities)
        {
            var models = new List<QuizModel>();
            foreach (var entity in entities)
                models.Add(Convert(entity));
            return models.AsEnumerable();
        }
        
        private static Quiz Convert(QuizModel model)
        {
            try
            {
                var entity = new Quiz
                {
                    Title = model.Title,
                    Questions = [],
                    AuthorName = model.Author,
                    AuthorId = model.AuthorId
                };

                foreach (var question in model.Questions)
                {
                    var entityOptions = new List<Option>();
                    foreach (var option in question.Options)
                        entityOptions.Add(new Option { Text = option.Text });

                    entity.Questions.Add(new Question
                    {
                        Options = entityOptions,
                        Title = question.Text,
                        CorrectAnswerIndex = question.CorrectAnswerIndex
                    });
                }
                return entity;
            } 
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        
        private static QuizModel Convert(Quiz entity)
        {
            var model = new QuizModel
            {
                Id = entity.Id,
                Title = entity.Title,
                Author = entity.AuthorName,
                AuthorId = entity.AuthorId
            };
            var questions = new List<QuestionModel>();
            foreach (Question question in entity.Questions)
            {
                var options = new List<OptionModel>();
                foreach (var option in question.Options)
                    options.Add(new OptionModel { Text = option.Text ?? "none" });
                questions.Add(new QuestionModel
                {
                    Options = options,
                    CorrectAnswerIndex = question.CorrectAnswerIndex,
                    Text = question.Title ?? "none"
                });
            }
            model.Questions = questions.ToArray();
            return model;
        }

        private static IEnumerable<QuizListItemModel> GetTitles(IEnumerable<QuizModel> models)
        {
            var list = new List<QuizListItemModel>();
            foreach (var item in models)
            {
                list.Add(new QuizListItemModel
                {
                    Title = item.Title,
                    Id = item.Id,
                    Rate = item.Rate,
                    AuthorId = item.AuthorId,
                    Author = item.Author
                });
            }
            return list.AsEnumerable();
        }
    }
}
