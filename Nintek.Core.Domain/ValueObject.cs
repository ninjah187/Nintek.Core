using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain
{
    public abstract class ValueObject : DomainObject
    {
        public abstract override string ToString();
    }
}
