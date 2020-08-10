using System.Collections.Generic;
using Neodenit.ActiveReader.Common.DataModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IAnswersService
    {
        IEnumerable<string> GetBestChoices(string correctAnswer, string correctAnswerFirstWord, IEnumerable<Stat> allChoices, int maxChoices, int answerLength);

        IEnumerable<string> GetBestChoicesLegacy(string correctAnswer, IEnumerable<Stat> allChoices, int maxChoices);

        IEnumerable<Stat> GetDoubleWordChoices(IEnumerable<Stat> statistics, string prefix);

        IEnumerable<Stat> GetSingleWordChoices(IEnumerable<Stat> statistics, string prefix);

        Stat GetTwoWordAnswer(Stat expression, Stat nextExpression);
    }
}