using Microsoft.EntityFrameworkCore;
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.ModelsInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public class UserAnswerService : IUserAnswerService
    {
        private readonly ApplicationDbContext dbContext;

        public UserAnswerService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void AddUserAnswer(string userId, int questionId, int answerId)
        {
            var userAnswer = new UserAnswer
            {
                IdentityUserId = userId,
                QuestionId = questionId,
                AnswerId = answerId
            };

            this.dbContext.UserAnswers.Add(userAnswer);
            this.dbContext.SaveChanges();
        }

        public void BulkAddUserAnswer(QuizInputModel quizInputModel)
        {
            var userAnswers = new List<UserAnswer>();

            foreach (var item in quizInputModel.Questions)
            {
                var userAnswer = new UserAnswer
                {
                    IdentityUserId = quizInputModel.UserId,
                    AnswerId = item.AnswerId,
                    QuestionId = item.QuestionId
                };

                userAnswers.Add(userAnswer);
            }

            this.dbContext.AddRange(userAnswers);
            this.dbContext.SaveChanges();
        }

        public int GetUserResult(string userId, int quizId)
        {
            var totalPoints = this.dbContext.UserAnswers
                .Where(x => x.IdentityUserId == userId
                         && x.Question.QuizId == quizId)               
                .Sum(x => x.Answer.Points);

            return totalPoints;
        }
    }
}
