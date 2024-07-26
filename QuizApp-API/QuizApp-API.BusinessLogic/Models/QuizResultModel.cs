namespace QuizApp_API.BusinessLogic.Models
{
    public class QuizResultModel
    {
        public string? Id { get; set; } = string.Empty;
        public string? QuizId { get; set; } = string.Empty;
        public string? UserId { get; set; } = string.Empty;
        public int Result { get; set; } = 0;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            QuizResultModel other = (QuizResultModel)obj;
            return Id == other.Id &&
                   QuizId == other.QuizId &&
                   UserId == other.UserId &&
                   Result == other.Result;
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + (Id?.GetHashCode() ?? 0);
                hash = hash * 23 + (QuizId?.GetHashCode() ?? 0);
                hash = hash * 23 + (UserId?.GetHashCode() ?? 0);
                hash = hash * 23 + Result.GetHashCode();
                return hash;
            }
        }
    }
}
