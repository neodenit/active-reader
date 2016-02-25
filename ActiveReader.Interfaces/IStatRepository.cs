using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ActiveReader.Interfaces
{
    public interface IStatRepository : IRepository<IStat>
    {
        IStat GetByPrefixAndSuffix(string prefix, string suffix);
    }
}
