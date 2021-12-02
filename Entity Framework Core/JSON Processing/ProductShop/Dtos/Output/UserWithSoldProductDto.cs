using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Output
{
    public class UserWithSoldProductDto
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<SoldProductDto> SoldProducts { get; set; }
    }
}
