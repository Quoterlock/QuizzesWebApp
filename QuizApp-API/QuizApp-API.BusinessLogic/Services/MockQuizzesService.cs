using QuizApp_API.BusinessLogic.Interfaces;
using QuizApp_API.BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WebApi.DataAccess
{
    public class MockQuizzesService : IQuizzesService
    {
        private List<QuizModel> quizzes = new List<QuizModel>();
        public MockQuizzesService() 
        {
            quizzes = Init();
        }

        public Task AddQuizAsync(QuizModel quiz)
        {
            throw new NotImplementedException();
        }

        public async Task<List<QuizModel>> GetAsync()
        {
            return quizzes;
        }

        public async Task<List<QuizModel>> GetAsync(int from, int to)
        {
            throw new NotImplementedException();
        }

        public async Task<QuizModel> GetQuizAsync(string id)
        {
            return quizzes.FirstOrDefault(i=>i.Id == id) ?? new QuizModel();
        }

        public async Task<List<QuizListItemModel>> GetTitlesAsync(int from, int to)
        {
            var list = await GetAsync(from, to);
            return GetTitles(list);
        }  

        public async Task<List<QuizListItemModel>> GetTitlesAsync()
        {
            var list = new List<QuizListItemModel>();
            return GetTitles(quizzes);
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

        private List<QuizModel> Init()
        {
            return new List<QuizModel> {
                new QuizModel {
                    Id = "0", Rate = 0, Title = "Counties and capitals Pt.1",
                    Questions = new QuestionModel[] {
                        new QuestionModel { CorrectAnswerIndex = 1, Text = "Damascus", 
                            Options = new List<OptionModel> {
                                new OptionModel { Text = "The Bahamas" },
                                new OptionModel { Text = "Syria" },
                                new OptionModel { Text = "Guyana" },
                                new OptionModel { Text = "Albania" }
                            }
                        },
                        new QuestionModel { CorrectAnswerIndex = 3, Text = "Helsinki",
                            Options = new List<OptionModel> {
                                new OptionModel { Text = "Nicaragua" },
                                new OptionModel { Text = "Sudan" },
                                new OptionModel { Text = "Norway" },
                                new OptionModel { Text = "Finland" }
                            }
                        },
                        new QuestionModel { CorrectAnswerIndex = 1, Text = "Santo Domingo",
                            Options = new List<OptionModel> {
                                new OptionModel { Text = "Antigua and Barbuda" },
                                new OptionModel { Text = "Dominican Republic" },
                                new OptionModel { Text = "Marshall Islands" },
                                new OptionModel { Text = "North Macedonia" }
                            }
                        },
                    }
                },
                new QuizModel {
                    Id = "1", Rate = 0, Title = "Counties and capitals Pt.2",
                    Questions = new QuestionModel[] {
                        new QuestionModel { CorrectAnswerIndex = 2, Text = "San Marino",
                            Options = new List<OptionModel> {
                                new OptionModel { Text = "Ghana" },
                                new OptionModel { Text = "Mongolia" },
                                new OptionModel { Text = "San Marino" },
                                new OptionModel { Text = "Belize" }
                            }
                        },
                        new QuestionModel { CorrectAnswerIndex = 0, Text = "Brasilia",
                            Options = new List<OptionModel> {
                                new OptionModel { Text = "Brazil" },
                                new OptionModel { Text = "Argentina" },
                                new OptionModel { Text = "Samoa" },
                                new OptionModel { Text = "China" }
                            }
                        },
                    }
                },
            };
        }
    }
}
