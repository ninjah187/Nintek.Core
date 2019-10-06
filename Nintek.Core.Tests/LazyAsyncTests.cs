using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Nintek.Core.Tests
{
    public class LazyAsyncTests
    {
        [Fact]
        public async Task CreatedWithConstructor_FromSyncSource_AwaitsCorrectValue()
        {
            LazyAsync<int> lazy = async () => 1;

            var value = await lazy();

            Assert.Equal(1, value);
        }
    }
}
