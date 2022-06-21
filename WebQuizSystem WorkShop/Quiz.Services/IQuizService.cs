using Quiz.Services.ModelsInfo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public interface IQuizService
    {
        int Add(string title);

        QuizInfoViewModel GetQuizById(int quizId);

        IEnumerable<UserQuizViewModel> GetQuizesByUsername(string username);

        void StartQuiz(string username, int quizId);
    }
}
