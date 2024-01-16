namespace QuizApp_API.DataAccess.Entities
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string OwnerId { get; set; }
        public byte[] ImageBytes { get; set; }
        public string DisplayName { get; set; }
    }
}
