using System.Threading.Tasks;
using ActiveReader.Models.Models;

namespace ActiveReader.Interfaces
{
    public interface IQuestionsService
    {
        Task<QuestionViewModel> GetQuestionAsync(int articleID, int lastAnswerPosition);
    }
}