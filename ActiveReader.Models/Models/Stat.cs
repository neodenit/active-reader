using System.ComponentModel.DataAnnotations.Schema;

namespace ActiveReader.Models.Models
{
    public class Stat
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        public virtual Article Article { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public int Count { get; set; }

        [NotMapped]
        public int SuffixPosition { get; set; }
    }
}