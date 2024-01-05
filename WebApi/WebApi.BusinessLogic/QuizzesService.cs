﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.BusinessLogic.Interfaces;
using WebApi.BusinessLogic.Models;
using WebApi.DataAccess.Data;
using WebApi.DataAccess.Entities;
using WebApi.DataAccess.Interfaces;

namespace WebApi.BusinessLogic
{
    public class QuizzesService : IQuizzesService
    {
        private readonly IQuizzesRepository _repository;
        public QuizzesService(IQuizzesRepository repository)
        { 
            _repository = repository;
        }

        public async Task AddQuizAsync(QuizModel quiz)
        {
            if(quiz == null) throw new ArgumentNullException(nameof(quiz));
            if (!string.IsNullOrEmpty(quiz.Title))
            {
                try
                {
                    await _repository.AddAsync(Convert(quiz));
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
            else throw new Exception("Question title is null or empty");
        }

        public async Task<List<QuizModel>> GetAsync()
        {
            var entities = await _repository.AsyncGet() ?? new List<Quiz>();
            var models = ConvertEntitiesToModels(entities);
            return models;
        }

        public async Task<List<QuizModel>> GetAsync(int from, int to)
        {
            if (from <= to)
            {
                var entities = await _repository.AsyncGet(from, to) ?? new List<Quiz>();
                var models = ConvertEntitiesToModels(entities);
                return models;
            }
            else throw new Exception("\"From\" cannot be larger that \"to\"");
        }

        public async Task<QuizModel> GetQuizAsync(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var entity = await _repository.AsyncGet(id);
                if (entity != null)
                    return Convert(entity);
                else 
                    throw new Exception("Entity doesn't exist with id:" + id);
            }
            else throw new ArgumentNullException(nameof(id));
        }

        public async Task<List<QuizListItemModel>> GetTitlesAsync(int from, int to)
        {
            var list = await GetAsync(from, to);
            return GetTitles(list);
        }

        public async Task<List<QuizListItemModel>> GetTitlesAsync()
        {
            var list = await GetAsync();
            return GetTitles(list);
        }

        private QuizModel Convert(Quiz entity)
        {
            var model = new QuizModel { Id = entity.Id, Title = entity.Title };
            var questions = new List<QuestionModel>();
            foreach(Question question in entity.Questions)
            {
                var options = new List<OptionModel>();
                foreach (var option in question.Options)
                options.Add(new OptionModel { Text = option.Text ?? "none" });
                questions.Add(new QuestionModel{
                    Options = options,
                    CorrectAnswerIndex = question.CorrectAnswerIndex,
                    Text = question.Title ?? "none"
                });
            }
            model.Questions = questions.ToArray();
            return model;
        }

        private List<QuizModel> ConvertEntitiesToModels(List<Quiz> entities)
        {
            var models = new List<QuizModel>();
            foreach (var entity in entities)
                models.Add(Convert(entity));
            return models;
        }
        private Quiz Convert(QuizModel model)
        {
            try
            {
                var entity = new Quiz();
                entity.Title = model.Title;
                entity.Questions = new List<Question>();
                foreach (var question in model.Questions)
                {
                    var entityOptions = new List<Option>();
                    foreach (var option in question.Options)
                        entityOptions.Add(new Option { Text = option.Text });

                    entity.Questions.Add(new Question
                    {
                        Options = entityOptions,
                        Title = question.Text,
                        CorrectAnswerIndex = question.CorrectAnswerIndex
                    });
                }
                return entity;
            } catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<QuizListItemModel> GetTitles(List<QuizModel> models)
        {
            var list = new List<QuizListItemModel>();
            foreach (var item in models)
            {
                list.Add(new QuizListItemModel
                {
                    Title = item.Title,
                    Id = item.Id,
                    Rate = item.Rate
                });
            }
            return list;
        }
    }
}
