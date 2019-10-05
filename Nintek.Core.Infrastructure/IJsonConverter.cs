﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Infrastructure
{
    public interface IJsonConverter
    {
        string Serialize(object data);
        object Deserialize(string json, Type type);
    }
}
