using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Domain;
using TasksManager.Data;

namespace TasksManager.Domain.Tasks
{
    public class TasksManager : ITasksManager
    {
        private readonly ITasksRepository _repository;
        public TasksManager(ITasksRepository repository)
        {
            _repository = repository;
        }

        public void CreateTask(TaskModel task)
        {
            if (task != null)
            {
                task.Id = Guid.NewGuid().ToString();
                _repository.Create(new TaskDTO
                {
                    Id = task.Id,
                    Text = task.Text,
                    Info = task.Info,
                    DueTo = task.DueTo,
                    Priority = task.Priority.ToString(),
                    IsCompleted = task.IsCompleted,
                    Tag = task.Tag,
                });
            }
            else throw new ArgumentNullException(nameof(task));
        }
        public void DeleteTask(TaskModel task)
        {
            if(task != null)
            {
                if (!string.IsNullOrEmpty(task.Id))
                {
                    _repository.Delete(task.Id);
                }
                else throw new Exception("Task_Id is null");
            } 
            else throw new ArgumentNullException(nameof(task));
        }
        public List<TaskModel> GetAll()
        {
            var tasks = _repository.Get() ?? [];
            return ConvertToModels(tasks);
        }
        public TaskModel GetById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var task = _repository.Get(task => task.Id == id).FirstOrDefault();
                if (task != null)
                    return ConvertToModel(task);
                else
                    throw new Exception("Task not found by id:" + id);
            }
            else throw new ArgumentNullException(nameof(id));
        }
        public List<TaskModel> GetByMatch(string field, object value)
        {
            var defaultTask = new TaskModel();
            var tasks = new List<TaskDTO>();
            if (field == nameof(defaultTask.Text))
                tasks = _repository.Get(task => task.Text == (string)value);
            if (field == nameof(defaultTask.IsCompleted))
                tasks = _repository.Get(task => task.IsCompleted == (bool)value);
            if (field == nameof(defaultTask.Info))
                tasks = _repository.Get(task => task.Info == (string)value);
            if (field == nameof(defaultTask.DueTo))
                tasks = _repository.Get(task => task.DueTo == (DateTime)value);
            if (field == nameof(defaultTask.Priority))
                tasks = _repository.Get(task => task.Priority == ((TaskPriority)value).ToString());
            if (field == nameof(defaultTask.Tag))
                tasks = _repository.Get(task => task.Tag == (string)value);
            return ConvertToModels(tasks ?? []);
        }
        public void UpdateTask(TaskModel task)
        {
            if (task != null)
            {
                if (!string.IsNullOrEmpty(task.Id))
                {
                    try
                    {
                        _repository.Update(new TaskDTO
                        {
                            Tag = task.Tag,
                            DueTo = task.DueTo,
                            Id = task.Id,
                            Info = task.Info,
                            IsCompleted = task.IsCompleted,
                            Text = task.Text,
                            Priority = task.Priority.ToString()
                        });
                    } catch (Exception ex)
                    {
                        throw new Exception("Tasks repository:" + ex.Message);
                    }
                }
                else throw new Exception("Task id is null.");
            }
            else throw new ArgumentNullException(nameof(task));
        }
        private static TaskModel ConvertToModel(TaskDTO task)
        {
            return new TaskModel
            {
                Id = task.Id,
                DueTo = task.DueTo,
                Info = task.Info,
                Text = task.Text,
                IsCompleted = task.IsCompleted,
                Tag = task.Tag,
                Priority = ParsePriority(task.Priority)
            };
        }
        private static List<TaskModel> ConvertToModels(List<TaskDTO> tasks)
        {
            var models = new List<TaskModel>();
            foreach (var task in tasks)
            {
                models.Add(ConvertToModel(task));
            }
            return models;
        }
        private static TaskPriority ParsePriority(string text)
        {
            TaskPriority priority = TaskPriority.None;
            if (text == TaskPriority.ImportantUrgent.ToString())
                priority = TaskPriority.ImportantUrgent;
            if (text == TaskPriority.ImportantNotUrgent.ToString())
                priority = TaskPriority.ImportantNotUrgent;
            if (text == TaskPriority.NotImportantUrgent.ToString())
                priority = TaskPriority.NotImportantUrgent;
            if (text == TaskPriority.NotImportantNotUrgent.ToString())
                priority = TaskPriority.NotImportantNotUrgent;
            return priority;
        }
    }
}
