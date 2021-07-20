using System;
using System.Collections.Generic;
using System.Text;

namespace Telephony.Exceptions
{
    public class InvalidPhoneNumber : Exception
    {
        private const string InvalidPhoneNumberMessage = "Invalid number!";
        public InvalidPhoneNumber() : base(InvalidPhoneNumberMessage)
        {

        }
    }
}
