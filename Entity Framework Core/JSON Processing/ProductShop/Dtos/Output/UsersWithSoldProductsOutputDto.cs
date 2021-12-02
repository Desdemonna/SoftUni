using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Output
{
    public class UsersWithSoldProductsOutputDto
    {
        public int UsersCount { get; set; }
        public IEnumerable<UserProductstDto> Users { get; set; }
    }
}
