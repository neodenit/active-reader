using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IConverter
    {
        IEnumerable<IWord> GetWords(IArticle article);
        string GetText(IEnumerable<IWord> words);
        IEnumerable<IStat> GetExpressions(IEnumerable<IWord> words);
        string GetPrefix(IEnumerable<string> words);
        string GetSuffix(string word);
        IEnumerable<string> GetWords(string text);
        IEnumerable<string> GetSpaces(string text);
        IEnumerable<string> SplitPrefix(string prefix);
    }
}