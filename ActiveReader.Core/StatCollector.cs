using ActiveReader.Interfaces;
using ActiveReader.Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Core
{
    public class StatCollector : IStatCollector
    {
        private readonly IRepository<Stat> repository;

        public StatCollector(IRepository<Stat> repository)
        {
            this.repository = repository;
        }

        public void Collect(string text, int articleID)
        {
            var words = System.Text.RegularExpressions.Regex.Split(text, @"\W+");

            var prefixLenght = 2;

            var prefixExpression = new Queue<string>(words.Take(prefixLenght));

            var rest = words.Skip(2);

            var delimeter = " ";

            foreach (var word in rest)
            {
                var prefix = string.Join(delimeter, prefixExpression);
                var suffix = word;

                var dbStat = repository.Get().SingleOrDefault(x => x.Prefix == prefix && x.Suffix == suffix && x.ArticleID == articleID);

                if (dbStat == null)
                {
                    var stat = new Stat { Prefix = prefix, Suffix = suffix, Count = 1, ArticleID = articleID };
                    repository.Create(stat);
                }
                else
                {
                    dbStat.Count++;
                }

                prefixExpression.Enqueue(word);
                prefixExpression.Dequeue();

                repository.Save();
            }
        }
    }
}
