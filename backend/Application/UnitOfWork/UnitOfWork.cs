using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Repositories;
using Domain.Interfaces;
using Persistence.Data;

namespace Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly NikeContext _context;

        private IProduct _Products;
        private IProductType _ProductTypes;
        private IState _States;
        private IRol _Rols;
        private IUser _Users;
        private IUserCar _UserCars;

        public UnitOfWork(NikeContext context)
        {
            _context = context;
        }

        public IProduct Products
        {
            get
            {
                if (_Products == null)
                {
                    _Products = new ProductRepository(_context);
                }
                return _Products;
            }
        }
        public IState States
        {
            get
            {
                if (_States == null)
                {
                    _States = new StateRepository(_context);
                }
                return _States;
            }
        }
        public IProductType ProductTypes
        {
            get
            {
                if (_ProductTypes == null)
                {
                    _ProductTypes = new ProductTypeRepository(_context);
                }
                return _ProductTypes;
            }
        }
        public IRol Rols
        {
            get
            {
                if (_Rols == null)
                {
                    _Rols = new RolRepository(_context);
                }
                return _Rols;
            }
        }
        public IUser Users
        {
            get
            {
                if (_Users == null)
                {
                    _Users = new UserRepository(_context);
                }
                return _Users;
            }
        }
        public IUserCar UserCars
        {
            get
            {
                if (_UserCars == null)
                {
                    _UserCars = new UserCarRepository(_context);
                }
                return _UserCars;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public Task<int> SaveAsync()
        {
            return _context.SaveChangesAsync();
        }
    }
}