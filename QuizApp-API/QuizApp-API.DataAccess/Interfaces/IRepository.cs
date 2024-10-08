﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizApp_API.DataAccess.Interfaces
{
    public interface IRepository<T>
    {
        Task AddAsync(T entity);
        Task DeleteAsync(string id);
        Task UpdateAsync(T entity);
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
    }
}
