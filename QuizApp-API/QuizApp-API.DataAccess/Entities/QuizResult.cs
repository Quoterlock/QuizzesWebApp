using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_API.DataAccess.Entities
{
    public class QuizResult
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public int Result { get; set; } = 0;
        public string QuizId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string TimeStamp { get; set; } = DateTime.Now.ToString();
    }
}
