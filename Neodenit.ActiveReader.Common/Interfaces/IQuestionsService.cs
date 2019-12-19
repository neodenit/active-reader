using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.Models;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IQuestionsService
    {
        Task<QuestionViewModel> GetQuestionAsync(int articleID, int lastAnswerPosition);
    }
}