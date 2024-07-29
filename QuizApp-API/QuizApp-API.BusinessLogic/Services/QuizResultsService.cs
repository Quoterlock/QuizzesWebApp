using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic.Services
{
    public class QuizResultsService(IQuizResultsRepository repository) 
        : IQuizResultsService
    {
        private readonly IQuizResultsRepository _repository = repository;

        public async Task<IEnumerable<QuizResultModel>> GetResultsByQuizIdAsync(string quizId)
        {
            if (!string.IsNullOrEmpty(quizId))
                return ConvertEntitiesToModels(await _repository.GetByQuizIdAsync(quizId));
            else
                throw new ArgumentNullException(nameof(quizId));
        }

        public async Task<IEnumerable<QuizResultModel>> GetResultsByUserIdAsync(string userId)
        {
            if (!string.IsNullOrEmpty(userId))
                return ConvertEntitiesToModels(await _repository.GetByUserIdAsync(userId));
            else
                throw new ArgumentNullException(nameof(userId));
        }

        public async Task SaveResultAsync(QuizResultModel quizResult)
        {
            ArgumentNullException.ThrowIfNull(quizResult);

            var entity = Convert(quizResult);
            entity.TimeStamp = DateTime.Now.ToString();

            await _repository.SaveResultAsync(entity);
        }

        private static QuizResult Convert(QuizResultModel model)
        {
            return new QuizResult
            {
                Id = model.Id ?? string.Empty,
                QuizId = model.QuizId ?? string.Empty,
                UserId = model.UserId ?? string.Empty,
                Result = model.Result,
            };
        }

        private static QuizResultModel Convert(QuizResult entity)
        {
            return new QuizResultModel
            {
                Id = entity.Id,
                UserId = entity.UserId,
                Result = entity.Result,
                QuizId = entity.QuizId,
            };
        }

        private static IEnumerable<QuizResultModel> ConvertEntitiesToModels(IEnumerable<QuizResult> entities)
        {
            var models = new List<QuizResultModel>();
            foreach (var entity in entities)
                models.Add(Convert(entity));
            return models.AsEnumerable();
        }
    }
}
