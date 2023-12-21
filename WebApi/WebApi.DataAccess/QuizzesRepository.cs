using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.DataAccess.Entities;
using WebApi.DataAccess.Interfaces;
using WebApi.DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace WebApi.DataAccess
{
    public class QuizzesRepository : IQuizzesRepository
    {
        private readonly WebApiContext _context;
        public QuizzesRepository(WebApiContext context) 
        { 
            _context = context;
        }
        public async Task<List<Quiz>> AsyncGet()
        {
            return (await _context.Quiz.ToListAsync()) ?? new List<Quiz>(); 
        }

        public async Task<List<Quiz>> AsyncGet(int start, int end)
        {
            return (await _context.Quiz.Skip(start).Take(end-start).ToListAsync()) ?? new List<Quiz>();
        }

        public async Task<Quiz?> AsyncGet(string id)
        {
            return (await _context.Quiz.FirstOrDefaultAsync(i=>i.Id == id));
        }
    }
}
