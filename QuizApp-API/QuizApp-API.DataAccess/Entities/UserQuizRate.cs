using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_API.DataAccess.Entities
{
    public class UserQuizRate
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = string.Empty;
        public string QuizId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public double Rate { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj is UserQuizRate other)
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
                int hashCode = 17;
                hashCode = hashCode * 23 + (Id?.GetHashCode() ?? 0);
                hashCode = hashCode * 23 + (QuizId?.GetHashCode() ?? 0);
                hashCode = hashCode * 23 + (UserId?.GetHashCode() ?? 0);
                hashCode = hashCode * 23 + Rate.GetHashCode();
                return hashCode;
            }
        }
    }
}
