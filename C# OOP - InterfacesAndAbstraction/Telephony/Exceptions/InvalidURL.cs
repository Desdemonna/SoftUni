using System;
using System.Collections.Generic;
using System.Text;

namespace Telephony.Exceptions
{
    public class InvalidURL : Exception
    {
        private const string InvalidURLMessage = "Invalid URL!";
        public InvalidURL() :base(InvalidURLMessage)
        {

        }
    }
}
