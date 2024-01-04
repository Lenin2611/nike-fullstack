using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class State : BaseEntity
    {
        public string Description { get; set; }

        public ICollection<Product> Products { get; set; }
    }
}