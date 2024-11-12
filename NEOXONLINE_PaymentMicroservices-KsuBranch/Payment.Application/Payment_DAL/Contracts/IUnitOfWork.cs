using Microsoft.EntityFrameworkCore.Storage;
using Payment.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Payment.Application.Payment_DAL.Contracts
{
    public interface IUnitOfWork
    {
        IDbContextTransaction BeginTransaction();

        IRepository<TEntity> GetRepository<TEntity>() where TEntity : class, IEntity;
        Task<int> SaveShangesAsync();
        IQueryable<TEntity> GetAllIncluding<TEntity>(params Expression<Func<TEntity, object>>[] includes) where TEntity : class;
    }
}
