using System.Collections.Generic;
using Neodenit.ActiveReader.Common.Attributes;

namespace Neodenit.ActiveReader.Common.ViewModels
{
    public class QuestionViewModel
    {
        [CheckOwner]
        public int ArticleID { get; set; }

        public int AnswerPosition { get; set; }

        public string StartingWords { get; set; }

        public IEnumerable<string> Variants { get; set; }

        public string Answer { get; set; }
    }
}
