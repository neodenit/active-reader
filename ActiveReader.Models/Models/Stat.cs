using ActiveReader.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ActiveReader.Models.Models
{
    public class Stat : IStat
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        public virtual IArticle Article { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public int Count { get; set; }

        [NotMapped]
        public int SuffixPosition { get; set; }
    }
}