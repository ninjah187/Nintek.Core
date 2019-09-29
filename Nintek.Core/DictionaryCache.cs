using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core
{
    // https://stackoverflow.com/questions/11617091/in-a-switch-vs-dictionary-for-a-value-of-func-which-is-faster-and-why
    public class DictionaryCache<TKey, TValue>
    {
        readonly Dictionary<TKey, TValue> _cache = new Dictionary<TKey, TValue>();

        readonly Func<TKey, TValue> _populate;

        public DictionaryCache(Func<TKey, TValue> populate)
        {
            _populate = populate;
        }

        public TValue Get(TKey key)
        {
            var success = _cache.TryGetValue(key, out var value);
            if (success)
            {
                return value;
            }
            value = _populate(key);
            _cache[key] = value;
            return value;
        }
    }
}
