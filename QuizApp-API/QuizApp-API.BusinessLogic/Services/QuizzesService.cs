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
        IRatesService ratesService,
        IUserProfilesService profilesService) : IQuizzesService
    {
        private readonly IQuizzesRepository _repository = repository;
        private readonly IQuizResultsService _resultsService = resultsService;
        private readonly IRatesService _ratesService = ratesService;
        private readonly IUserProfilesService _profilesService = profilesService;

        public async Task AddQuizAsync(QuizModel quiz)
        {
            ArgumentNullException.ThrowIfNull(quiz);

            if (string.IsNullOrEmpty(quiz.Title))
                throw new ArgumentException("Quiz title is null or empty");
            if (string.IsNullOrEmpty(quiz.Author.Owner.Id))
                throw new ArgumentException("Quiz author is null or empty");

            try
            {        
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
                ?? throw new ArgumentException("Entity doesn't exist with id:" + id);

            var model = Convert(entity);
            // get results
            model.Results = await _resultsService.GetResultsByQuizIdAsync(model.Id);
            // get author profile
            model.Author = (await _profilesService.GetRangeAsync(entity.AuthorUserId))[0];
            // get rates
            var rates = await _ratesService.GetRatesAsync(model.Id);
            if (!rates.IsNullOrEmpty())
                model.Rate = rates.Average(e => e.Rate);

            return model;
        }

        public async Task<List<QuizListItemModel>> GetTitlesAsync(int from, int to)
        {
            var list = await GetRangeAsync(from, to);
            return GetTitles(list);
        }

        public async Task<List<QuizListItemModel>> GetTitlesAsync()
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
                await _resultsService.RemoveByQuizIdAsync(id);
                await _ratesService.RemoveByQuizIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<QuizListItemModel>> GetAllTitlesByUserIdAsync(string userId)
        {
            // get quizzes
            var entities = (await _repository.GetByUserIdAsync(userId)).ToList();
            var models = await ConvertEntitiesToModels(entities);
            return GetTitles(models);
        }

        public async Task<List<QuizListItemModel>> SearchAsync(string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(nameof(value));
            var entities = (await _repository.SearchAsync(value)).ToList();
            var models = await ConvertEntitiesToModels(entities);
            return GetTitles(models);
        }
        
        public async Task<int> GetAllUserCompletedAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));

            return (await _resultsService.GetResultsByUserIdAsync(userId)).GroupBy(r => r.QuizId).Select(v => v.Key).Count();
        }

        private async Task<List<QuizModel>> AddRatesForQuizzes(List<QuizModel> quizzes)
        {
            string[] ids = quizzes.Select(e => e.Id).ToArray();
            var rates = await _ratesService.GetRatesAsync(ids);
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

        private async Task<List<QuizModel>> GetAllAsync()
        {
            var entities = (await _repository.GetAllAsync()).ToList();
            return await ConvertEntitiesToModels(entities);
        }

        private async Task<List<QuizModel>> GetRangeAsync(int from, int to)
        {
            if (from > to)
                throw new Exception("\"From\" cannot be larger that \"to\"");
            var entities = (await _repository.GetRangeAsync(from, to)).ToList();
            return await ConvertEntitiesToModels(entities);
        }

        private async Task<List<QuizModel>> ConvertEntitiesToModels(List<Quiz> entities)
        {
            var ids = entities.Select(e => e.AuthorUserId).ToArray();
            var authors = (await _profilesService.GetRangeAsync(ids)).ToArray();
            var models = new List<QuizModel>();

            foreach (var entity in entities)
            {
                var model = Convert(entity);
                model.Author = authors.FirstOrDefault(e=>e.Owner.Id == entity.AuthorUserId) ?? new();
                models.Add(model);
            }

            return await AddRatesForQuizzes(models);
        }
        
        private static Quiz Convert(QuizModel model)
        {
            try
            {
                var entity = new Quiz
                {
                    Id = model.Id,
                    Title = model.Title,
                    Questions = [],
                    AuthorUserId = model.Author.Owner.Id,
                    CreationDate = model.CreationDate,
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

        private static List<QuizListItemModel> GetTitles(List<QuizModel> models)
        {
            var list = new List<QuizListItemModel>();
            foreach (var item in models)
            {
                list.Add(new QuizListItemModel
                {
                    Title = item.Title,
                    Id = item.Id,
                    Rate = item.Rate,
                    Author = item.Author,
                });
            }
            return list;
        }
    }
}
