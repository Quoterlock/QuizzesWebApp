using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess.Entities;

namespace WebApi.DataAccess.Interfaces
{
    public interface IQuizzesRepository
    {
        Task<List<Quiz>> AsyncGet();
        Task<List<Quiz>> AsyncGet(int start, int end);
        Task<Quiz?> AsyncGet(string id);
    }
}
