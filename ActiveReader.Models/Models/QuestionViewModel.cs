using ActiveReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Models.Models
{
    public class QuestionViewModel : IQuestionViewModel
    {
        public int ArticleID { get; set; }

        public int AnswerPosition { get; set; }

        public string StartingWords { get; set; }

        public IEnumerable<string> Variants { get; set; }

        public string Answer { get; set; }
    }
}
