using System;
using System.Collections.Generic;
using System.Linq;
using Telephony.Exceptions;

namespace Telephony.Models
{
    public class Smartphone : Interfaces.ICallable, Interfaces.IBrowsable
    {
        public string Browse(string url)
        {
            if (url.Any(x => char.IsDigit(x)))
            {
                throw new InvalidURL();
            }
            return $"Browsing: {url}!";
        }

        public string Call(string phoneNumber)
        {
            if (!phoneNumber.All(x => char.IsDigit(x)))
            {
                throw new InvalidPhoneNumber();
            }

            return $"Calling... {phoneNumber}";
        }
    }
}
