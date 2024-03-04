using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TasksManager.Data
{
    public class TasksRepository : ITasksRepository
    {
        public void Create(TaskDTO task)
        {
            throw new NotImplementedException();
        }

        public void Delete(string id)
        {
            throw new NotImplementedException();
        }

        public List<TaskDTO> Get(Func<TaskDTO, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public List<TaskDTO> Get()
        {
            throw new NotImplementedException();
        }

        public TaskDTO GetById(string id)
        {
            throw new NotImplementedException();
        }

        public void Update(TaskDTO task)
        {
            throw new NotImplementedException();
        }
    }
}
