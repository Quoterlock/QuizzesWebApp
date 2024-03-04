using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Entities
{
    public class QuizResult
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Result { get; set; } = 0;
        public string QuizId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
    }
}
