using System;
using System.Collections.Generic;
using System.Linq;
using Telephony.Exceptions;

namespace Telephony.Models
{
    public class StationaryPhone : Interfaces.ICallable
    {
        public string Call(string phoneNumber)
        {
            if (!phoneNumber.All(x => char.IsDigit(x)))
            {
                throw new InvalidPhoneNumber();
            }

            return $"Dialing... {phoneNumber}";
        }
    }
}
