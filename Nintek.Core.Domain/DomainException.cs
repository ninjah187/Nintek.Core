﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain
{
    public class DomainException : Exception
    {
        public DomainException(string message)
            : base(message)
        {
        }
    }
}
