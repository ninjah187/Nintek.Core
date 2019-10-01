using Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Data.Dapper
{
    public static class IUnitOfWorkExtensions
    {
        public static async Task<int> ExecuteAsync(this IUnitOfWork unitOfWork, string sql, object parameter = null)
        {
            var connection = await unitOfWork.Connector.Connect();
            return await connection.ExecuteAsync(sql, parameter, unitOfWork.Transaction);
        }

        public static async Task<TResult> ExecuteScalarAsync<TResult>(this IUnitOfWork unitOfWork, string sql, object parameter = null)
        {
            var connection = await unitOfWork.Connector.Connect();
            return await connection.ExecuteScalarAsync<TResult>(sql, parameter, unitOfWork.Transaction);
        }

        public static async Task<TResult> QueryFirstOrDefaultAsync<TResult>(this IUnitOfWork unitOfWork, string sql, object parameter = null)
        {
            var connection = await unitOfWork.Connector.Connect();
            return await connection.QueryFirstOrDefaultAsync<TResult>(sql, parameter, unitOfWork.Transaction);
        }

        public static async Task<TResult> QuerySingleAsync<TResult>(this IUnitOfWork unitOfWork, string sql, object parameter = null)
        {
            var connection = await unitOfWork.Connector.Connect();
            return await connection.QuerySingleAsync<TResult>(sql, parameter, unitOfWork.Transaction);
        }

        public static async Task<TResult> QuerySingleOrDefaultAsync<TResult>(this IUnitOfWork unitOfWork, string sql, object parameter = null)
        {
            var connection = await unitOfWork.Connector.Connect();
            return await connection.QuerySingleOrDefaultAsync<TResult>(sql, parameter, unitOfWork.Transaction);
        }

        public static async Task<IEnumerable<TResult>> QueryAsync<TResult>(this IUnitOfWork unitOfWork, string sql, object parameter = null)
        {
            var connection = await unitOfWork.Connector.Connect();
            return await connection.QueryAsync<TResult>(sql, parameter, unitOfWork.Transaction);
        }
    }
}
