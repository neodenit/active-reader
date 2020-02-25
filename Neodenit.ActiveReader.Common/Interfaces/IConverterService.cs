using System.Collections.Generic;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IConverterService
    {
        IEnumerable<Word> GetWords(Article article);

        string GetText(IEnumerable<Word> words);

        IEnumerable<Stat> GetExpressions(IEnumerable<Word> words, int prefixLength, bool ignoreCase);

        string GetPrefix(IEnumerable<string> words, bool ignoreCase);

        string GetSuffix(string word, bool ignoreCase);

        string NormalizeWord(string word, bool ignoreCase);

        IEnumerable<string> GetWords(string text);

        IEnumerable<string> GetSpaces(string text);

        IEnumerable<string> SplitPrefix(string prefix);
    }
}