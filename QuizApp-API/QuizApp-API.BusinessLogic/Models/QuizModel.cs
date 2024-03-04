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
    }

    public class QuestionModel
    {
        public string Text { get; set; } = string.Empty;
        public int CorrectAnswerIndex { get; set; } = 0;
        public List<OptionModel> Options { get; set; } = new List<OptionModel>();
    }

    public class OptionModel
    {
        public string Text { get; set; } = string.Empty;
    }
}
