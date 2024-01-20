using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace QuizApp_API.DataAccess.Entities
{
    public class Quiz
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AuthorId { get; set; } = string.Empty;
        public string AuthorName { get; set; } = string.Empty;
        public string? Title { get; set; }
        public List<Question>? Questions { get; set; }
    }

    public class Question
    {
        public string? Title { get; set; }
        public int CorrectAnswerIndex { get; set; }
        public List<Option>? Options { get; set; }
    }

    public class Option
    {
        public string? Text { get; set; }
    }
}
