using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Data
{
    public class TaskDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public DateTime DueTo { get; set; }
        public bool IsCompleted { get; set; }
        public string Priority { get; set; } = "None";
        public string Tag { get; set; } = "none";
    }
}
