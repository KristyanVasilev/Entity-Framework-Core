using Quiz.Data;
using Quiz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly ApplicationDbContext dbContext;

        public AnswerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(string title, bool isCorrect, int points, int questionId)
        {
            var answer = new Answer
            {
                Title = title,
                IsCorrect = isCorrect,
                Points = points,
                QuestionId = questionId
            };

            this.dbContext.Answers.Add(answer);
            this.dbContext.SaveChanges();
        }
    }
}
