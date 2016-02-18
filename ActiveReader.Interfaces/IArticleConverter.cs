namespace ActiveReader.Interfaces
{
    public interface IArticleConverter
    {
        void SaveArticle(string text, int articleID);
    }
}