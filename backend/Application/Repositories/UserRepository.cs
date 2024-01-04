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
    public class UserRepository : GenericRepository<User>, IUser
    {
        private readonly NikeContext _context;

        public UserRepository(NikeContext context) : base(context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users
                        .Include(u => u.Rols)
                        .FirstOrDefaultAsync(x => x.Username.ToLower() == username.ToLower());
        }
    }
}