using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserCar : BaseEntity
    {
        public int QuantityInCar { get; set; } = 0;
        public int IdUserFk { get; set; }
        public int IdProductFk { get; set; }

        public User Users { get; set; }
        public Product Products { get; set; }
    }
}