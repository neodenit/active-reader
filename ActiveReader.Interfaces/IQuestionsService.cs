namespace ActiveReader.Interfaces
{
    public interface IQuestionsService
    {
        int GetPosition(int position);
        string GetStartingWords(int articleID, int position);
    }
}