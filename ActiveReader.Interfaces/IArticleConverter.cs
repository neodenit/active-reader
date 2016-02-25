using System.Collections.Generic;

namespace ActiveReader.Interfaces
{
    public interface IArticleConverter
    {
        void SaveArticle(string text, int articleID);
        string GetText(IEnumerable<IWord> words);
    }
}