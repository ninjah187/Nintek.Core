using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events
{
    public interface IEventEmitter
    {
        EventQueue Events { get; }
    }
}
