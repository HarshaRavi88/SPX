using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using SPX.DataProvider.Models;

namespace SPX.DataProvider
{
    public class GenericRepository<TEntity> where TEntity : class
    {

        internal SpxProductsContext Context;
        internal DbSet<TEntity> DbSet;


        public GenericRepository(SpxProductsContext context)
        {
            this.Context = context;
            this.DbSet = context.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual IEnumerable<TEntity> Get()
        {
            IQueryable<TEntity> query = DbSet;
            return query.ToList();
        }


        public virtual TEntity GetByID(object id)
        {
            return DbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }


        public virtual void Delete(TEntity entityToDelete)
        {
            if (Context.Entry(entityToDelete).State == EntityState.Detached)
            {
                DbSet.Attach(entityToDelete);
            }
            DbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            DbSet.Attach(entityToUpdate);
            Context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        public IEnumerable<TEntity> QuerySet(Expression<Func<TEntity, bool>> filter, string children)
        {
            return DbSet.Include(children).Where(filter);
        }

        //public TEntity Get(Func<TEntity, Boolean> where)
        //{
        //    return DbSet.Where(where).FirstOrDefault<TEntity>();
        //}


      

    }
}
