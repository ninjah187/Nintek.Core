using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Events.Handling
{
    public enum EventualConsistentState
    {
        /// <summary>
        /// Event is new and ready for processing.
        /// </summary>
        Processable,

        /// <summary>
        /// Event processing failed at least once, but should be retried in the future.
        /// </summary>
        Failed,

        /// <summary>
        /// Event processing exceeded retry limit and is definitely failed.
        /// </summary>
        Error
    }
}
