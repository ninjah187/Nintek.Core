using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core
{
    public class Clock
    {
        public DateTime Now => DateTime.Now;
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
