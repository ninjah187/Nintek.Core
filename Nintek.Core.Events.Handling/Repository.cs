using Nintek.Core.Data;
using Nintek.Core.Data.Dapper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Events.Handling
{
    public abstract class Repository
    {
        protected IUnitOfWork UnitOfWork { get; }

        protected Repository(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork;
        }

        protected async Task<int> NextSequenceValue(string sequenceName)
        {
            var value = await UnitOfWork.QueryFirstOrDefaultAsync<int>(
                "SELECT nextval(@sequenceName)", new { sequenceName });
            return value;
        }

        protected Task<int> NextIdValue(string tableName)
        {
            return NextSequenceValue($"{tableName}IdSequence");
        }
    }
}
