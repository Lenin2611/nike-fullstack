using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : BaseEntity
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

        public ProductType ProductTypes { get; set; }
        public State States { get; set; }
        public ICollection<User> Users { get; set; } = new HashSet<User>();
        public ICollection<UserCar> UserCars { get; set; }
    }
}