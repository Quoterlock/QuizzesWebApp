﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using QuizApp_API.DataAccess.Entities;
using QuizApp_API.DataAccess.Interfaces;

namespace QuizApp_API.DataAccess.Repositories
{
    public class QuizzesRepository : IQuizzesRepository
    {
        /*
        private readonly WebApiContext _context;
        public QuizzesRepository(WebApiContext context) 
        { 
            _context = context;
        }

        public async Task AddAsync(Quiz quiz)
        {
            if(quiz != null)
            {
                await _context.Quizzes.AddAsync(quiz);
                await _context.SaveChangesAsync();
            }
            else throw new ArgumentNullException(nameof(quiz));
        }

        public async Task<List<Quiz>> AsyncGet()
        {
            var entities = (await _context.Quizzes.Include(q => q.Questions).ToListAsync()) ?? new List<Quiz>();

            foreach (var entity in entities)
                entity.Questions = await GetFullQuestions(entity.Questions);

            return entities;
        }

        public async Task<List<Quiz>> AsyncGet(int start, int end)
        {
            var entities = (await _context.Quizzes
                .Include(q=>q.Questions)
                .Skip(start).Take(end-start).ToListAsync()) 
                ?? new List<Quiz>();
            foreach(var entity in entities)
                entity.Questions = await GetFullQuestions(entity.Questions);
            
            return entities;
        }

        public async Task<Quiz?> AsyncGet(string id)
        {
            var result = await _context.Quizzes.Include(q=>q.Questions).FirstOrDefaultAsync(i=>i.Id == id);
            result.Questions = await GetFullQuestions(result.Questions);
            return result;
        }

        private async Task<List<Question>> GetFullQuestions(List<Question> questions)
        {
            var result = new List<Question>();
            foreach(var question in questions)
               result.Add(await _context.Questions.Include(q=>q.Options).FirstOrDefaultAsync(q => q.Id == question.Id));
            
            return result;
        }
        */
        public Task AddAsync(Quiz entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(Quiz entity)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Quiz>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Quiz> GetByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Quiz>> GetRangeAsync(int start, int end)
        {
            throw new NotImplementedException();
        }

        public Task Update(Quiz entity)
        {
            throw new NotImplementedException();
        }
    }
}
