using System.Collections.Generic;
namespace Quiz.Services.ModelsInfo
{
    public class QuizInfoViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<QuestionViewModel> Questions { get; set; }
    }
}
