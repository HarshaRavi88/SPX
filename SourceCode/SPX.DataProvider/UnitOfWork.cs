using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SPX.DataProvider.Models;

namespace SPX.DataProvider
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        private SpxProductsContext _context = null;
        private GenericRepository<Product> _productTblRepository;
        private GenericRepository<Review> _reviewTblRepository;
        private GenericRepository<User> _userRepository;
    
        
        public UnitOfWork()
        {
            _context = new SpxProductsContext();
        }

       
        public GenericRepository<Product> ProductTblRepository
        {
            get
            {
                if (this._productTblRepository == null)
                    this._productTblRepository = new GenericRepository<Product>(_context);
                return _productTblRepository;
            }
        }


        public GenericRepository<Review> ReviewsTblRepository
        {
            get
            {
                if (this._reviewTblRepository == null)
                    this._reviewTblRepository = new GenericRepository<Review>(_context);
                return _reviewTblRepository;
            }
        }

        public GenericRepository<User> UserRepository
        {
            get
            {
                if (this._userRepository == null)
                    this._userRepository = new GenericRepository<User>(_context);
                return _userRepository;
            }
        }
        
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                throw;
            }

        }
        
        #region Dispose
        private bool disposed = false;
      
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {  
                    _context.Dispose();
                }
            }
            this.disposed = true;
        }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
