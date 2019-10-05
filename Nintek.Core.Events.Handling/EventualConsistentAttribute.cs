using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    /// <summary>
    /// Marks given event handler as eventual consistent, meaning it will be processed in background, in separate transaction.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EventualConsistentAttribute : Attribute
    {
    }
}
