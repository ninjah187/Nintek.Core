using System;
using System.Collections.Generic;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public class EmailAddress : Text
    {
        public EmailAddress(Required<string> value) : base(value)
        {
            if (!EmailAddressValidator.IsValid(value))
            {
                throw new DomainException($"Invalid e-mail address format: {value}.");
            }
        }
    }
}
