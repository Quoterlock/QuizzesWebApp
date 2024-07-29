namespace QuizApp_API.DataAccess.Entities
{
    public class Quiz
    {
        public string Id { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = [];
        public string CreationDate { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is Quiz other)
            {
                return Id == other.Id &&
                       AuthorId == other.AuthorId &&
                       AuthorName == other.AuthorName &&
                       Title == other.Title &&
                       CreationDate == other.CreationDate &&
                       Questions.SequenceEqual(other.Questions);
            }
            return false;
        }
        public override int GetHashCode()
        {
            unchecked // Allow overflow to wrap around
            {
                int hash = 17;
                hash = hash * 23 + (Id?.GetHashCode() ?? 0);
                hash = hash * 23 + (AuthorId?.GetHashCode() ?? 0);
                hash = hash * 23 + (AuthorName?.GetHashCode() ?? 0);
                hash = hash * 23 + (Title?.GetHashCode() ?? 0);
                hash = hash * 23 + (CreationDate?.GetHashCode() ?? 0);
                hash = hash * 23 + (Questions?.Aggregate(0, (current, question) => current * 23 + (question?.GetHashCode() ?? 0)) ?? 0);
                return hash;
            }
        }

    }

    public class Question
    {
        public string Title { get; set; } = string.Empty;
        public int CorrectAnswerIndex { get; set; }
        public List<Option> Options { get; set; } = [];

        public override bool Equals(object? obj)
        {
            if (obj is Question other)
            {
                return Title == other.Title &&
                       CorrectAnswerIndex == other.CorrectAnswerIndex &&
                       Options.SequenceEqual(other.Options);
            }
            return false;
        }
        public override int GetHashCode()
        {
            unchecked // Allow overflow to wrap around
            {
                int hash = 17;
                hash = hash * 23 + (Title?.GetHashCode() ?? 0);
                hash = hash * 23 + CorrectAnswerIndex.GetHashCode();
                hash = hash * 23 + (Options?.Aggregate(0, (current, option) => current * 23 + (option?.GetHashCode() ?? 0)) ?? 0);
                return hash;
            }
        }
    }

    public class Option
    {
        public string Text { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is Option other)
            {
                return Text == other.Text;
            }
            return false;
        }
        public override int GetHashCode()
        {
            return Text?.GetHashCode() ?? 0;
        }
    }
}
