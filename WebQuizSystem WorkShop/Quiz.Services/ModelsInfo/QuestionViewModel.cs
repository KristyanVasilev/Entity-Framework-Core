using System.Collections.Generic;

namespace Quiz.Services.ModelsInfo
{
    public class QuestionViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<AnswerViewModel> Answers { get; set; }
    }
}