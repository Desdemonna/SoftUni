using System;
using System.Collections.Generic;
using System.Text;

namespace ProductShop.Dtos.Output
{
    public class ProductsDto
    {
        public int Count { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}
