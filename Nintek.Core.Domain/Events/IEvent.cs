using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Events
{
    public interface IEvent
    {
        int Id { get; }
    }
}
