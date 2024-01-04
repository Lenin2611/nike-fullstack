using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Dtos
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }
        public string Image { get; set; }
        public string Size { get; set; }
        public double Price { get; set; }
        public int Stock { get; set; }
        public int StockMin { get; set; }
        public int StockMax { get; set; }
        public int IdProductTypeFk { get; set; }
        public int IdStateFk { get; set; }
    }
}