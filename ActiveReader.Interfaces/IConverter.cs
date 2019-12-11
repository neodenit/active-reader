﻿using System.Collections.Generic;
using ActiveReader.Models.Models;

namespace ActiveReader.Interfaces
{
    public interface IConverter
    {
        IEnumerable<Word> GetWords(Article article);
        string GetText(IEnumerable<Word> words);
        IEnumerable<Stat> GetExpressions(IEnumerable<Word> words);
        string GetPrefix(IEnumerable<string> words);
        string GetSuffix(string word);
        IEnumerable<string> GetWords(string text);
        IEnumerable<string> GetSpaces(string text);
        IEnumerable<string> SplitPrefix(string prefix);
    }
}