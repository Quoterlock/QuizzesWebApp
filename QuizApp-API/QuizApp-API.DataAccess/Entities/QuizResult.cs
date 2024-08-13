using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_API.DataAccess.Entities
{
    public class QuizResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; } = string.Empty;
        public int Result { get; set; } = 0;
        public string QuizId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is QuizResult other)
            {
                return Id == other.Id &&
                       Result == other.Result &&
                       QuizId == other.QuizId &&
                       UserId == other.UserId &&
                       TimeStamp == other.TimeStamp;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = 17;
                hashCode = hashCode * 23 + (Id?.GetHashCode() ?? 0);
                hashCode = hashCode * 23 + Result.GetHashCode();
                hashCode = hashCode * 23 + (QuizId?.GetHashCode() ?? 0);
                hashCode = hashCode * 23 + (UserId?.GetHashCode() ?? 0);
                hashCode = hashCode * 23 + (TimeStamp?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}
