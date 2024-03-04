using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Data
{
    public interface ITasksRepository
    {
        TaskDTO GetById(string id);
        List<TaskDTO> Get(Func<TaskDTO, bool> predicate);
        List<TaskDTO> Get();
        void Delete(string id);
        void Create(TaskDTO task);
        void Update(TaskDTO task);
    }
}
