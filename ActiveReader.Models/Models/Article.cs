using ActiveReader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveReader.Models.Models
{
    public class Article : IArticle
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public string Text { get; set; }
    }
}