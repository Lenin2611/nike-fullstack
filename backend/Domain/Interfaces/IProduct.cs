using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IProduct : IGenericRepository<Product>
    {
        Task<List<Product>> GetProductByType(string type);
        Task<List<object>> GetProductInCar(string username);
    }
}