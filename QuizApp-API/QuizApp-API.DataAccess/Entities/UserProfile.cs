namespace QuizApp_API.DataAccess.Entities
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public byte[] ImageBytes { get; set; } = [];
        public string DisplayName { get; set; } = string.Empty;
    }
}
