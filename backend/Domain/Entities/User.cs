using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : BaseEntity
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public DateTime TokenCreated { get; set; }
        public DateTime TokenExpires { get; set; }

        public ICollection<Rol> Rols { get; set; } = new HashSet<Rol>();
        public ICollection<UserRol> UserRols { get; set; }
        public ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public ICollection<UserCar> UserCars { get; set; }
    }
}