using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic.Services
{
    public class QuizResultsService(
        IQuizResultsRepository repository,
        IUserProfilesService profilesService) 
        : IQuizResultsService
    {
        private readonly IQuizResultsRepository _repository = repository;
        private readonly IUserProfilesService _profilesService = profilesService;

        public async Task<IEnumerable<QuizResultModel>> GetResultsByQuizIdAsync(string quizId)
        {
            if (!string.IsNullOrEmpty(quizId))
                return await ConvertEntitiesToModels(await _repository.GetByQuizIdAsync(quizId));
            else
                throw new ArgumentNullException(nameof(quizId));
        }

        public async Task<IEnumerable<QuizResultModel>> GetResultsByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            return await ConvertEntitiesToModels(await _repository.GetByUserIdAsync(userId));
        }

        public async Task SaveResultAsync(string quizId, string userId, int result)
        {
            ArgumentException.ThrowIfNullOrEmpty(quizId);
            ArgumentException.ThrowIfNullOrEmpty(userId);

            var quizResult = new QuizResult
            {
                QuizId = quizId,
                Result = result,
                UserId = userId
            };
            
            await _repository.SaveResultAsync(quizResult);
        }

        private static QuizResult Convert(QuizResultModel model)
        {
            return new QuizResult
            {
                Id = model.Id ?? string.Empty,
                QuizId = model.QuizId ?? string.Empty,
                UserId = model.UserProfile.Owner.Id ?? string.Empty,
                Result = model.Result,
                TimeStamp = model.TimeStamp
            };
        }

        private static QuizResultModel Convert(QuizResult entity)
        {
            return new QuizResultModel
            {
                Id = entity.Id,
                Result = entity.Result,
                QuizId = entity.QuizId,
                TimeStamp = entity.TimeStamp,
            };
        }

        private async Task<IEnumerable<QuizResultModel>> ConvertEntitiesToModels(IEnumerable<QuizResult> entities)
        {
            var models = new List<QuizResultModel>();
            var profiles = await _profilesService.GetRangeAsync(entities.Select(e => e.UserId).ToArray());
            foreach (var entity in entities)
            {
                var model = Convert(entity);
                model.UserProfile = profiles.Find(p => p.Owner.Id == entity.UserId)??new();
                models.Add(model);
            }
            return models.AsEnumerable();
        }

        public async Task RemoveByQuizIdAsync(string quizId)
        {
            if (string.IsNullOrEmpty(quizId))
                throw new ArgumentNullException(nameof(quizId));
            await _repository.RemoveByQuizIdAsync(quizId);
        }
    }
}
