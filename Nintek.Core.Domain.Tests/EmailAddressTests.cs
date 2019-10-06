using Nintek.Core.Domain.Common;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Nintek.Core.Domain.Tests
{
    public class EmailAddressTests
    {
        [Fact]
        public void Constructor_WithValidParameter_SuccessfullyCreatesObject()
        {
            var value = "test@mail.com";
            var email = new EmailAddress(value);

            Assert.Equal(value, email);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("@")]
        [InlineData("test@")]
        [InlineData("@mail.com")]
        [InlineData("@mail")]
        [InlineData("mail.com")]
        [InlineData("test")]
        public void Constructor_WithInvalidParameter_ThrowsDomainException(string value)
        {
            Assert.Throws<DomainException>(() =>
            {
                new EmailAddress(value);
            });
        }
    }
}
