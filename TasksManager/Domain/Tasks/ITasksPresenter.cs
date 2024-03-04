using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain
{
    public interface ITasksPresenter
    {
        List<TaskModel> GetAllTasks();
        List<TaskModel> GetTodayTasks();
        Dictionary<DayOfWeek, List<TaskModel>> GetWeekTasks();
        List<TaskModel> GetByTag(string tag);
        List<TaskModel> SortByPriority(List<TaskModel> tasks, TaskPriority priority);
        List<TaskModel> SortByPriority(Dictionary<DayOfWeek, List<TaskModel>> tasks, TaskPriority priority);
        List<TaskModel> SortByDueToDate(List<TaskModel> tasks);
        List<TaskModel> FilterOnlyIncomplete(List<TaskModel> tasks);
    }
}
