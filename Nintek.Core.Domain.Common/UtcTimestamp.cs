using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class UtcTimestamp : Timestamp
    {
        public UtcTimestamp(DateTime value) : base(value.ToUniversalTime())
        {
        }
    }
}
