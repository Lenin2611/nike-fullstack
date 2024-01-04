using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Persistence.Data;

namespace Application.Repositories
{
    public class UserCarRepository : GenericRepository<UserCar>, IUserCar
    {
        private readonly NikeContext _context;

        public UserCarRepository(NikeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserCar> GetUserCarByIds(int user, int product)
        {
            return await _context.UserCars
                        .FirstOrDefaultAsync(x => x.IdUserFk == user && x.IdProductFk == product);
        }
    }
}