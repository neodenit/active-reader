using System.ComponentModel.DataAnnotations.Schema;

namespace Neodenit.ActiveReader.Common.DataModels
{
    public class Stat
    {
        public int Id { get; set; }

        public int ArticleId { get; set; }

        public virtual Article Article { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public int Count { get; set; }

        [NotMapped]
        public double Probability { get; set; }

        [NotMapped]
        public int SuffixPosition { get; set; }

        [NotMapped]
        public string SuffixFirstWord { get; set; }
    }
}