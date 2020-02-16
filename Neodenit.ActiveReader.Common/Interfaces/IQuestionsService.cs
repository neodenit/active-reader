using System.Threading.Tasks;
using Neodenit.ActiveReader.Common.ViewModels;

namespace Neodenit.ActiveReader.Common.Interfaces
{
    public interface IQuestionsService
    {
        Task<QuestionViewModel> GetQuestionAsync(int articleId, int lastAnswerPosition);
    }
}