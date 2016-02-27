using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IQuestionsService
    {
        Task<IQuestionViewModel> GetQuestionAsync(int articleID, int lastAnswerPosition);
    }
}