using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Nintek.Core
{
    public class LazyAsync<T>
    {
        readonly Provider<T> _provider;

        T _value;
        bool _isLoaded;

        public LazyAsync(Provider<T> provider)
        {
            _provider = provider;
        }

        public async Task<T> GetValue()
        {
            if (!_isLoaded)
            {
                _value = await _provider();
                _isLoaded = true;
            }
            return _value;
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return GetValue().GetAwaiter();
        }
    }

    public static class LazyAsync
    {
        public static LazyAsync<T> From<T>(Provider<T> provider) => new LazyAsync<T>(provider);
    }
}
