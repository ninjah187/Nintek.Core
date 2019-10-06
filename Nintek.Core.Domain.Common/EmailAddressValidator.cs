using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Nintek.Core.Domain.Common
{
    public static class EmailAddressValidator
    {
        static readonly EmailAddressAttribute _attribute = new EmailAddressAttribute();

        public static bool IsValid(string email)
        {
            return _attribute.IsValid(email);
        }
    }
}
