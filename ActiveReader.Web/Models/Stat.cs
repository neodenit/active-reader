using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ActiveReader.Web.Models
{
    public class Stat
    {
        public int ID { get; set; }
        public string Prefix { get; set; }
        public string Suffix { get; set; }
        public int Number { get; set; }
    }
}