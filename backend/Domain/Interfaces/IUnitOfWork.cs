using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUnitOfWork
    {
        public IProduct Products { get; }
        public IProductType ProductTypes { get; }
        public IState States { get; }
        public IRol Rols { get; }
        public IUser Users { get; }
        public IUserCar UserCars { get; }

        Task<int> SaveAsync();
    }
}