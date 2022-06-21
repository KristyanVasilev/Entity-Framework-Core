
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.ModelsInfo;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext dbContext;

        public QuizService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public int Add(string title)
        {
            var quiz = new Models.Quiz
            {
                Title = title
            };

            this.dbContext.Quizes.Add(quiz);
            this.dbContext.SaveChanges();

            return quiz.Id;
        }

        public QuizInfoViewModel GetQuizById(int quizId)
        {
            var quiz = this.dbContext.Quizes
                .Include(x => x.Questions)
                .ThenInclude(x => x.Answers)
                .FirstOrDefault(x => x.Id == quizId);

            var quizModel = new QuizInfoViewModel
            {
                Id = quizId,
                Title = quiz.Title,
                Questions = quiz.Questions.Select(q => new QuestionViewModel
                {
                    Id = q.Id,
                    Title = q.Title,
                    Answers = q.Answers.Select(a => new AnswerViewModel
                    {
                        Id = a.Id,
                        Title = a.Title
                    })
                })             
            };

            return quizModel;
        }

        public IEnumerable<UserQuizViewModel> GetQuizesByUsername(string username)
        {
            var quizes = dbContext.Quizes
                .Select(x => new UserQuizViewModel
                {
                    QuizId = x.Id,
                    Title = x.Title
                })
                .ToList();

            foreach (var quiz in quizes)
            {
                var questionsCount = dbContext.UserAnswers
                    .Count(ua => ua.IdentityUser.UserName == username
                              && ua.Question.QuizId == quiz.QuizId);

                if (questionsCount == 0)
                {
                    quiz.Status = QuizStatus.NotStarted;
                    continue;
                }

                var answeredQuestionsCount = dbContext.UserAnswers
                   .Count(ua => ua.IdentityUser.UserName == username
                             && ua.Question.QuizId == quiz.QuizId
                             && ua.AnswerId.HasValue);

                if (answeredQuestionsCount == questionsCount)
                {
                    quiz.Status = QuizStatus.Finished;
                }
                else
                {
                    quiz.Status = QuizStatus.InProgress;
                }
            }

            return quizes;
        }

        public void StartQuiz(string username, int quizId)
        {
            if (dbContext.UserAnswers.Any(x => x.IdentityUserId == username && x.Question.QuizId == quizId))
            {
                return;
            }
            var userId = dbContext.Users
                .Where(x => x.UserName == username)
                .Select(x => x.Id)
                .FirstOrDefault();

            var questions = dbContext.Questions
                .Where(x => x.QuizId == quizId)
                .Select(x => new { x.Id })
                .ToList();

            foreach (var question in questions)
            {
                dbContext.UserAnswers.Add(new UserAnswer
                {
                    AnswerId = null,
                    IdentityUserId = userId,
                    QuestionId = question.Id
                });
            }

            dbContext.SaveChanges();
        }
    }
}
