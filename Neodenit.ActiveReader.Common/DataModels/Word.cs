namespace Neodenit.ActiveReader.Common.DataModels
{
    public class Word
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public int Position { get; set; }

        public string CorrectedWord { get; set; }

        public string OriginalWord { get; set; }

        public string NextSpace { get; set; }
    }
}
