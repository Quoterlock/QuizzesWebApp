﻿namespace QuizApp_API.BusinessLogic.Models
{
    public class QuizListItemModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public double Rate { get; set; } = 0;
        public UserProfileInfo Author { get; set; } = new();    

        public override bool Equals(object? obj)
        {
            if (obj is QuizListItemModel other)
            {
                return Id == other.Id &&
                       Title == other.Title &&
                       Rate.Equals(other.Rate) &&
                       Author == other.Author;
            }

            return false;
        }
        public override int GetHashCode()
        {
            // Combine hash codes of all properties to generate a hash code for the instance.
            unchecked
            {
                int hash = 17;
                hash = hash * 23 + (Id?.GetHashCode() ?? 0);
                hash = hash * 23 + (Title?.GetHashCode() ?? 0);
                hash = hash * 23 + Rate.GetHashCode();
                hash = hash * 23 + (Author?.GetHashCode() ?? 0);
                return hash;
            }
        }
    }
}
