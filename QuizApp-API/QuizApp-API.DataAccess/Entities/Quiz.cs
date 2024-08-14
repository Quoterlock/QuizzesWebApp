namespace QuizApp_API.DataAccess.Entities
{
    public class Quiz
    {
        public string Id { get; set; } = string.Empty;
        public string AuthorUserId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public List<Question> Questions { get; set; } = [];
        public string CreationDate { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is not Quiz other)
                return false;

            return Id == other.Id &&
                   AuthorUserId == other.AuthorUserId &&
                   Title == other.Title &&
                   CreationDate == other.CreationDate &&
                   EqualityComparer<List<Question>>.Default.Equals(Questions, other.Questions);
        }

        public override int GetHashCode()
        {
            // Use a hash code combining strategy.
            var hash = new HashCode();
            hash.Add(Id);
            hash.Add(AuthorUserId);
            hash.Add(Title);
            hash.Add(CreationDate);
            hash.Add(Questions);
            return hash.ToHashCode();
        }
    }

    public class Question
    {
        public string Title { get; set; } = string.Empty;
        public int CorrectAnswerIndex { get; set; }
        public List<Option> Options { get; set; } = [];

        public override bool Equals(object? obj)
        {
            if (obj is not Question other)
                return false;

            return Title == other.Title &&
                   CorrectAnswerIndex == other.CorrectAnswerIndex &&
                   EqualityComparer<List<Option>>.Default.Equals(Options, other.Options);
        }

        public override int GetHashCode()
        {
            // Use a hash code combining strategy.
            var hash = new HashCode();
            hash.Add(Title);
            hash.Add(CorrectAnswerIndex);
            hash.Add(Options);
            return hash.ToHashCode();
        }

    }

    public class Option
    {
        public string Text { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is not Option other)
                return false;

            return Text == other.Text;
        }

        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
