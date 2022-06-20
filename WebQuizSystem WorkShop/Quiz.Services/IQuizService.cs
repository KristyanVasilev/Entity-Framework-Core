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
        void Add(string title);

        QuizInfoViewModel GetQuizById(int quizId);
    }
}
