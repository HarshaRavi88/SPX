using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPX.DataProvider
{
    public interface IUnitOfWork
    {
        void Save();
    }
}
