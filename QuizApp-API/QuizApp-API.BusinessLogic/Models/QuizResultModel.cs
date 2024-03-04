namespace QuizApp_API.BusinessLogic.Models
{
    public class QuizResultModel
    {
        public string? Id { get; set; } = string.Empty;
        public string? QuizId { get; set; } = string.Empty;
        public string? UserId { get; set; } = string.Empty;
        public int Result { get; set; } = 0;

        /*
        public override bool Equals(object? obj)
        {
            if (obj == null) return false;
            var model = obj as QuizResultModel;
            return model.Id.Equals(this.Id) && 
                model.Result == this.Result && 
                model.UserId.Equals(this.UserId) && 
                model.QuizId.Equals(this.QuizId);
        }
        */
    }
}
