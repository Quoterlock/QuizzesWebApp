﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.BusinessLogic.Models
{
    public class QuizListItemModel
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public double Rate { get; set; } = 0;
        public string Author { get; set; } = string.Empty;
        public string AuthorId { get; set; } = string.Empty;
    }
}
