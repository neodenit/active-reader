namespace ActiveReader.Interfaces
{
    public interface IWord
    {
        int ArticleID { get; set; }
        string CorrectedWord { get; set; }
        int ID { get; set; }
        string NextSpace { get; set; }
        string OriginalWord { get; set; }
        int Position { get; set; }
    }
}