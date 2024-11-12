using Payment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Payment_DAL.Contracts
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Delete(TEntity entity);

        IQueryable<TEntity> AsQueryable();

        IQueryable<TEntity> AsReadOnlyQueryable();

         Task<TEntity>  InsertOrUpdate(
           Expression<Func<TEntity, bool>> predicate,
           TEntity entity
       );

    }
}
