namespace QuizApp_API.DataAccess.Entities
{
    public class UserProfile
    {
        public string Id { get; set; } = string.Empty;
        public string OwnerId { get; set; } = string.Empty;
        public string ImageId { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is UserProfile other)
            {
                return Id == other.Id &&
                       OwnerId == other.OwnerId &&
                       ImageId == other.ImageId &&
                       DisplayName == other.DisplayName;
            }
            return false;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 31 + (Id?.GetHashCode() ?? 0);
                hash = hash * 31 + (OwnerId?.GetHashCode() ?? 0);
                hash = hash * 31 + (ImageId?.GetHashCode() ?? 0);
                hash = hash * 31 + (DisplayName?.GetHashCode() ?? 0);
                return hash;
            }
        }

    }
}
