using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Services
{
    public interface IAnswerService
    {
        void Add(string title, bool isCorrect, int points, int questionId);
    }
}
