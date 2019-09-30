using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Data
{
    public interface IUnitOfWork
    {
        IDbConnector Connector { get; }
        IDbTransaction Transaction { get; }
        Task Commit();
        event Action<IUnitOfWork> Commited;
    }
}
