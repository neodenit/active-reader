namespace Neodenit.ActiveReader.Common.DataModels
{
    public class Article
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }

        public string Owner { get; set; }

        public int Position { get; set; }

        public int PrefixLength { get; set; }

        public int MaxChoices { get; set; }
    }
}