using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Domain
{
    public interface ITasksManager
    {
        void CreateTask(TaskModel task);
        void DeleteTask(TaskModel task);
        void UpdateTask(TaskModel task);
        TaskModel GetById(string id);
        List<TaskModel> GetAll();
        List<TaskModel> GetByMatch(string field, object value);
    }
}
