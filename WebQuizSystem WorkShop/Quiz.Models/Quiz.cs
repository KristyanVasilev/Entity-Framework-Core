using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quiz.Models
{
    public class Quiz
    {
        public Quiz()
        {
            this.Questions = new HashSet<Question>();
            this.UserAnswers = new HashSet<UserAnswer>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<Question> Questions { get; set; }

        public ICollection<UserAnswer> UserAnswers { get; set; }

    }
}
