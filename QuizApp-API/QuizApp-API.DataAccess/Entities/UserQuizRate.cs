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
    }
}
