using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProduct
    {
        private readonly NikeContext _context;

        public ProductRepository(NikeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Product>> GetProductByType(string type)
        {
            return await (from product in _context.Products
                        join producttype in _context.ProductTypes on product.IdProductTypeFk equals producttype.Id
                        where producttype.Type == type
                        select product).ToListAsync(); 
        }

        public async Task<List<object>> GetProductInCar(string username)
        {
            return await (from product in _context.Products
                        join state in _context.States on product.IdStateFk equals state.Id
                        join usercar in _context.UserCars on product.Id equals usercar.IdProductFk
                        join user in _context.Users on usercar.IdUserFk equals user.Id
                        where state.Description.ToLower() == "incar" && user.Username == username
                        select new {
                            id = product.Id,
                            name = product.Name,
                            image = product.Image,
                            size = product.Size,
                            price = product.Price,
                            quantityInCar = usercar.QuantityInCar
                        }).ToListAsync<object>();
        }
    }
}