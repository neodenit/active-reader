using ActiveReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Models.Models
{
    public class Word : IWord
    {
        public int ID { get; set; }

        public int ArticleID { get; set; }

        public virtual Article Article { get; set; }

        public int Position { get; set; }

        public string CorrectedWord { get; set; }

        public string OriginalWord { get; set; }

        public string NextSpace { get; set; }
    }
}
