using QuizApp_API.DataAccess.Entities;

namespace QuizApp_API.BusinessLogic.Models
{
    public class QuizModel
    {
        public string Id { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public QuestionModel[] Questions { get; set; } = new QuestionModel[0];
        public double Rate { get; set; } = 0;
        public IEnumerable<QuizResultModel> Results { get; set; } = [];
        public string CreationDate { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            QuizModel other = (QuizModel)obj;
            return Id == other.Id &&
                   Author == other.Author &&
                   AuthorId == other.AuthorId &&
                   Title == other.Title &&
                   Questions.SequenceEqual(other.Questions) && // SequenceEqual compares arrays content
                   Rate == other.Rate &&
                   CreationDate == other.CreationDate &&
                   Results.SequenceEqual(other.Results); // SequenceEqual compares IEnumerable content
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Id.GetHashCode();
                hash = hash * 23 + Author.GetHashCode();
                hash = hash * 23 + AuthorId.GetHashCode();
                hash = hash * 23 + Title.GetHashCode();
                hash = hash * 23 + CreationDate.GetHashCode();
                foreach (var question in Questions)
                {
                    hash = hash * 23 + question.GetHashCode();
                }
                hash = hash * 23 + Rate.GetHashCode();
                foreach (var result in Results)
                {
                    hash = hash * 23 + result.GetHashCode();
                }
                return hash;
            }
        }
    }

    public class QuestionModel
    {
        public string Text { get; set; } = string.Empty;
        public int CorrectAnswerIndex { get; set; } = 0;
        public List<OptionModel> Options { get; set; } = new List<OptionModel>();

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            QuestionModel other = (QuestionModel)obj;
            return Text == other.Text &&
                   CorrectAnswerIndex == other.CorrectAnswerIndex &&
                   Options.SequenceEqual(other.Options); // SequenceEqual compares lists content
        }
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Text.GetHashCode();
                hash = hash * 23 + CorrectAnswerIndex.GetHashCode();
                foreach (var option in Options)
                {
                    hash = hash * 23 + option.GetHashCode();
                }
                return hash;
            }
        }

    }

    public class OptionModel
    {
        public string Text { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            OptionModel other = (OptionModel)obj;
            return Text == other.Text;
        }
        public override int GetHashCode()
        {
            return Text.GetHashCode();
        }
    }
}
