using Newtonsoft.Json;
using Nintek.Core.Events.Handling;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Infrastructure
{
    public class NewtonsoftJsonConverter : IJsonConverter
    {
        readonly JsonSerializerSettings _settings;

        public NewtonsoftJsonConverter(JsonSerializerSettings settings)
        {
            _settings = settings;
        }

        public object Deserialize(string json, Type type) => JsonConvert.DeserializeObject(json, type, _settings);

        public string Serialize(object data) => JsonConvert.SerializeObject(data, _settings);
    }
}
