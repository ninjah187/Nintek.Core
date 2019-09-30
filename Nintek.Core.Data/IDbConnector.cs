using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Data
{
    public interface IDbConnector
    {
        Task<IDbConnection> Connect();
    }
}
