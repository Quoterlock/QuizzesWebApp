using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Models
{
    public class QuizRateModel
    {
        public string Id { get; set; } = string.Empty;
        public string QuizId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public double Rate { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj is QuizRateModel other)
            {
                return Id == other.Id &&
                       QuizId == other.QuizId &&
                       UserId == other.UserId &&
                       Rate.Equals(other.Rate);
            }
            return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (Id?.GetHashCode() ?? 0);
                hash = hash * 31 + (QuizId?.GetHashCode() ?? 0);
                hash = hash * 31 + (UserId?.GetHashCode() ?? 0);
                hash = hash * 31 + Rate.GetHashCode();
                return hash;
            }
        }
    }
}
