using System.Collections.Generic;

namespace ActiveReader.Interfaces
{
    public interface IQuestionViewModel
    {
        string Answer { get; set; }
        int AnswerPosition { get; set; }
        int ArticleID { get; set; }
        string StartingWords { get; set; }
        IEnumerable<string> Variants { get; set; }
    }
}