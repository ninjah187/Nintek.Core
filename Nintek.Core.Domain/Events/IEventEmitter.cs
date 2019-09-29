using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Events
{
    public interface IEventEmitter
    {
        EventQueue Events { get; }
    }
}
