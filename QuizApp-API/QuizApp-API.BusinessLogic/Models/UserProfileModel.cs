namespace QuizApp_API.BusinessLogic.Models
{
    public class UserProfileModel
    {
        public string Id { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public ProfileOwnerInfo Owner { get; set; } = new ProfileOwnerInfo();
        public List<QuizListItemModel> CreatedQuizzes { get; set; } = [];
        public int CompletedQuizzesCount { get; set; } = 0;
        public override bool Equals(object? obj)
        {
            if (obj is UserProfileModel other)
            {
                bool areEqual = Id == other.Id &&
                                DisplayName == other.DisplayName &&
                                Equals(Owner, other.Owner) &&
                                CreatedQuizzes.Count == other.CreatedQuizzes.Count &&
                                CompletedQuizzesCount == other.CompletedQuizzesCount;
                for (int i = 0; i < CreatedQuizzes.Count; i++)
                {
                    if (!CreatedQuizzes[i].Equals(other.CreatedQuizzes[i]))
                    {
                        areEqual = false;
                        break;
                    }
                }

                return areEqual;
            }

            return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Id?.GetHashCode() ?? 0);
                hash = hash * 23 + (DisplayName?.GetHashCode() ?? 0);
                hash = hash * 23 + (Owner?.GetHashCode() ?? 0);
                hash = hash * 23 + (CompletedQuizzesCount.GetHashCode());

                foreach (var quiz in CreatedQuizzes)
                {
                    hash = hash * 23 + quiz.GetHashCode();
                }

                return hash;
            }
        }
    }

    public class ProfileOwnerInfo
    {
        public string Id { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;

        public override bool Equals(object? obj)
        {
            if (obj is ProfileOwnerInfo other)
            {
                return Id == other.Id && Username == other.Username;
            }

            return false;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Id?.GetHashCode() ?? 0);
                hash = hash * 23 + (Username?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
