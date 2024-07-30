using Microsoft.IdentityModel.Tokens;
using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.BusinessLogic.Services
{
    public class RatesService : IRatesService
    {
        private readonly IRatesRepository _ratesRepository;
        public RatesService(IRatesRepository ratesRepository) 
        {
            _ratesRepository = ratesRepository;
        }

        public async Task AddRate(string quizId, string userId, double rate)
        {
            if (string.IsNullOrEmpty(quizId))
                throw new ArgumentNullException(nameof(quizId));
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentNullException(nameof(userId));
            if (rate < 0 || rate > 100)
                throw new ArgumentOutOfRangeException(nameof(rate));

            await _ratesRepository.AddAsync(new UserQuizRate
            {
                QuizId = quizId,
                UserId = userId,
                Rate = rate
            });
        }

        public async Task DeleteRate(string rateId)
        {
            if (string.IsNullOrEmpty(rateId))
                throw new ArgumentNullException(nameof(rateId));
            await _ratesRepository.DeleteAsync(rateId);
        }

        public async Task UpdateRate(string rateId, double rate)
        {
            if (string.IsNullOrEmpty(rateId))
                throw new ArgumentNullException(nameof(rateId));
            if (rate < 0 || rate > 100)
                throw new ArgumentOutOfRangeException(nameof(rate));
            var old = await _ratesRepository.GetByIdAsync(rateId);
            old.Rate = rate;
        }

        public async Task<List<QuizRateModel>> GetRatesAsync(params string[] quizIds)
        {
            ArgumentNullException.ThrowIfNull(quizIds);
            if(quizIds.Length == 0)
                return [];

            var entities = await _ratesRepository.Get(quizIds);
            var models = new List<QuizRateModel>();
            foreach (var entity in entities)
                models.Add(Convert(entity));
            return models;
        }

        private QuizRateModel Convert(UserQuizRate entity)
        {
            return new QuizRateModel
            {
                Id = entity.Id,
                QuizId = entity.QuizId,
                UserId = entity.UserId,
                Rate = entity.Rate
            };
        }
    }
}
