using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Events.Handling
{
    public interface IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}
