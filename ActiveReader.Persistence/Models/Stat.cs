using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveReader.Persistence.Models
{
    public class Stat
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        public virtual Article Article { get; set; }

        public string Prefix { get; set; }

        public string Suffix { get; set; }

        public int Number { get; set; }
    }
}