using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Models.Models
{
    public class QuestionViewModel
    {
        public int ArticleID { get; set; }

        public int Position { get; set; }

        public string StartingWords { get; set; }

        public IEnumerable<string> Variants { get; set; }

        public string Answer { get; set; }
    }
}
