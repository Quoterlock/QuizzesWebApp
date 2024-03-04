using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain;

namespace TasksManager.Domain.Tasks
{
    public class TasksPresenter : ITasksPresenter
    {
        private readonly ITasksManager _tasks;
        public TasksPresenter(ITasksManager tasks) 
        {
            _tasks = tasks;
        }

        public List<TaskModel> FilterOnlyIncomplete(List<TaskModel> tasks)
        {
            return _tasks.GetByMatch(nameof(TaskModel.IsCompleted), false);
        }
        public List<TaskModel> GetAllTasks()
        {
            return _tasks.GetAll();
        }

        public List<TaskModel> GetByTag(string tag)
        {
            return _tasks.GetByMatch(nameof(TaskModel.Tag), tag);
        }

        public List<TaskModel> GetTodayTasks()
        {
            return _tasks.GetByMatch(nameof(TaskModel.DueTo), DateTime.Now);
        }

        public Dictionary<DayOfWeek, List<TaskModel>> GetWeekTasks()
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> SortByDueToDate(List<TaskModel> tasks)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> SortByPriority(List<TaskModel> tasks, TaskPriority priority)
        {
            throw new NotImplementedException();
        }

        public List<TaskModel> SortByPriority(Dictionary<DayOfWeek, List<TaskModel>> tasks, TaskPriority priority)
        {
            throw new NotImplementedException();
        }
    }
}
