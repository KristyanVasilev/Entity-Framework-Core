
using Quiz.Data;
using Quiz.Models;
using Quiz.Services.ModelsInfo;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Quiz.Services
{
    public class QuizService : IQuizService
    {
        private readonly ApplicationDbContext dbContext;

        public QuizService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public void Add(string title)
        {
            var quiz = new Models.Quiz
            {
                Title = title
            };

            this.dbContext.Quizes.Add(quiz);
            this.dbContext.SaveChanges();
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
    }
}
