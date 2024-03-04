namespace TaskManager.Domain
{
    public class TaskModel
    {
        public string Id { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Info { get; set; } = string.Empty;
        public DateTime DueTo { get; set; }
        public bool IsCompleted { get; set; }
        public TaskPriority Priority { get; set; } = TaskPriority.None;
        public string Tag { get; set; } = "none";
    }

    public enum TaskPriority
    {
        ImportantUrgent,
        ImportantNotUrgent,
        NotImportantUrgent,
        NotImportantNotUrgent,
        None
    }
}
