using Quiz.Data;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class QuestionService : IQestionService
    {
        private readonly ApplicationDbContext dbContext;

        public QuestionService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public int Add(string title, int quizId)
        {
            var question = new Question
            {
                Title = title,
                QuizId = quizId
            };

            this.dbContext.Questions.Add(question);
            this.dbContext.SaveChanges();

            return question.Id;
        }
    }
}
