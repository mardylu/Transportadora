using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Common;

namespace System.Data.Entity.Repository
{
    public interface IRepositoryBase
    {
        bool Add<R>(R entity) where R : class;
        bool AddMultiple<R>(List<R> entities) where R : class;
        bool AddMultipleWithCommit<R>(List<R> entities) where R : class;
        bool AddOrUpdate<R>(R entity) where R : class;
        bool AddOrUpdateMultiple<R>(List<R> entities) where R : class;
        bool AddOrUpdateMultipleCommit<R>(List<R> entities) where R : class;

        void CommitTransaction(bool startNewTransaction = false);

        Int32 Count<R>() where R : class;

        void Dispose();

        bool Delete<R>(R entity) where R : class;
        bool DeleteMultiple<R>(List<R> entities) where R : class;
        bool DeleteMultipleWithCommit<R>(List<R> entities) where R : class;

        void Detach(object entity);
        void Detach(List<object> entities);

        IQueryable<R> Find<R>(Expression<Func<R, bool>> where) where R : class;
        IQueryable<R> Find<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class;
        
        R First<R>(Expression<Func<R, bool>> where) where R : class;
        R FirstOrDefault<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class;

        IQueryable<R> GetAll<R>() where R : class;
        IQueryable<R> GetAll<R>(params Expression<Func<R, object>>[] includes) where R : class;

        DbConnection GetConnection();

        void SaveChanges();
        void SetIdentityCommand();
        void SetRethrowExceptions(bool rehtrowExceptions);
        void SetTransactionType(TransactionTypes transactionType);
        void SetUseTransaction(bool useTransaction);
        
        R Single<R>(Expression<Func<R, bool>> where) where R : class;
        R SingleOrDefault<R>(Expression<Func<R, bool>> where) where R : class;
        R SingleOrDefault<R>(Expression<Func<R, bool>> where, Expression<Func<R, object>> include) where R : class;

        bool Update<R>(R entity) where R : class;
        bool UpdateMultiple<R>(List<R> entities) where R : class;
        bool UpdateMultipleWithCommit<R>(List<R> entities) where R : class;
    }
}
