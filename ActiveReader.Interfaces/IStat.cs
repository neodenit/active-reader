namespace ActiveReader.Interfaces
{
    public interface IStat
    {
        IArticle Article { get; set; }
        int ArticleID { get; set; }
        int ID { get; set; }
        int Count { get; set; }
        string Prefix { get; set; }
        string Suffix { get; set; }
    }
}