namespace ActiveReader.Interfaces
{
    public interface IArticle
    {
        int ID { get; set; }
        string Text { get; set; }
        string Title { get; set; }
    }
}