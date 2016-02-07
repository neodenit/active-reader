using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveReader.Web.Models
{
    public class Article
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
    }
}