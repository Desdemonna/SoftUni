using WebServer.Server.HTTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Server.Responses
{
    public class BadRequestResponse : Response
    {
        public BadRequestResponse() : base(StatusCode.BadRequest)
        {
        }
    }
}
