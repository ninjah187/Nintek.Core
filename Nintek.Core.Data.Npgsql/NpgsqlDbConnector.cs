using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Data.Npgsql
{
    public class NpgsqlDbConnector : IDbConnector, IDisposable
    {
        public event Action<IDbConnection> Connected;

        readonly string _connectionString;

        NpgsqlConnection _connection;

        public NpgsqlDbConnector(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IDbConnection> Connect()
        {
            if (_connection == null)
            {
                _connection = new NpgsqlConnection(_connectionString);
                await _connection.OpenAsync();
                Connected?.Invoke(_connection);
            }
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Dispose();
        }
    }
}
