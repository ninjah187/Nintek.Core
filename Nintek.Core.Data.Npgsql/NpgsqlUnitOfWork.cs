using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Data.Npgsql
{
    public class NpgsqlUnitOfWork : IUnitOfWork
    {
        public event Action<IUnitOfWork> Commited;

        public IDbConnector Connector { get; }

        public IDbTransaction Transaction => _transaction;

        NpgsqlTransaction _transaction;

        public NpgsqlUnitOfWork(IDbConnector connector)
        {
            Connector = connector;
            Connector.Connected += connection =>
            {
                StartTransactionIfNeeded((NpgsqlConnection) connection);
            };
        }

        public async Task Commit()
        {
            if (_transaction == null)
            {
                return;
            }
            await _transaction.CommitAsync();
            _transaction = null;
            Commited?.Invoke(this);
        }

        void StartTransactionIfNeeded(NpgsqlConnection connection)
        {
            if (_transaction != null)
            {
                return;
            }
            _transaction = connection.BeginTransaction();
        }
    }
}
