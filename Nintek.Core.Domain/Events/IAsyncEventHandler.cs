using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core.Domain.Events
{
    public interface IAsyncEventHandler<TEvent> where TEvent : IEvent
    {
        Task Handle(TEvent @event);
    }
}
