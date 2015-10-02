using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Data.Metadata.Edm;
using System.Transactions;
using System.Data.EntityClient;

namespace System.Data.Entity.Repository
{
    public class RepositoryBase<T> : IRepositoryBase, IDisposable
        where T : DbContext
    {
        public DbContext Model;

        private TransactionTypes _transactionType = TransactionTypes.DbTransaction;
        private TransactionScope _transactionScope;
        private IsolationLevel _isolationLevel = IsolationLevel.ReadUncommitted;
        private DbTransaction _transaction;
        private DbConnection _connection;

        private bool _proxyCreationEnabled = false;
        private bool _rethrowExceptions = true;
        private bool _useTransaction = true;

        private int _commandTimeout = 300;

        private string _connectionString = string.Empty;

        public event RepositoryBaseExceptionHandler RepositoryBaseExceptionRaised;
        public delegate void RepositoryBaseExceptionHandler(Exception exception);

        internal void InitializeRepository()
        {
            if (Model == null)
            {
                DbContext instance = (DbContext)Activator.CreateInstance(typeof(T));
                ((IObjectContextAdapter)instance).ObjectContext.CommandTimeout = 300;
                Model = instance;

                if (!string.IsNullOrEmpty(_connectionString))
                {
                    Model.Database.Connection.ConnectionString = _connectionString;
                }

                Model.Configuration.ProxyCreationEnabled = _proxyCreationEnabled;

                _connection = ((IObjectContextAdapter)Model).ObjectContext.Connection;
                _connection.Open();
            }
            else
            {
                Model.Configuration.LazyLoadingEnabled = false;
            }
        }

        public RepositoryBase()
        {
            InitializeRepository();
        }

        public RepositoryBase(bool throwExceptions, string connectionString = "", TransactionTypes transactionType = TransactionTypes.DbTransaction, IsolationLevel isolationLevel = IsolationLevel.ReadUncommitted,
            bool useTransactions = true, bool proxyCreationEnabled = false, int commandTimeout = 300)
        {
            _rethrowExceptions = throwExceptions;
            _useTransaction = useTransactions;
            _proxyCreationEnabled = proxyCreationEnabled;
            _commandTimeout = commandTimeout;
            _isolationLevel = IsolationLevel.ReadUncommitted;
            _connectionString = connectionString;

            InitializeRepository();
        }

        public RepositoryBase(RepositoryBaseConfiguration configuration)
        {
            _rethrowExceptions = configuration.RethrowExceptions;
            _useTransaction = configuration.UseTransaction;
            _proxyCreationEnabled = configuration.ProxyCreationEnabled;
            _commandTimeout = configuration.CommandTimeout;
            _isolationLevel = configuration.IsolationLevel;
            _connectionString = configuration.ConnectionString;

            InitializeRepository();
        }

        public bool Add<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    SetEntity<R>()
                        .Add(entity);

                    SaveChanges();

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            RepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }

            });
            
            return result;
        }

        public bool AddMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = Add<R>(e));

            return result;
        }

        public bool AddMultipleWithCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e =>
            {
                result = Add<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public bool AddOrUpdate<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        if (entry.State == EntityState.Detached)
                        {
                            entry.State = EntityState.Added;
                        }
                        else
                        {
                            entry.State = EntityState.Modified;
                        }
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    SaveChanges();

                    result = true;
                }
                catch (Exception)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);
                }
                finally
                { }

            });

            return result;
        }

        public bool AddOrUpdateMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = AddOrUpdate<R>(e));

            return result;
        }

        public bool AddOrUpdateMultipleCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => {
                result = AddOrUpdate<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public void CommitTransaction(bool startNewTransaction = false)
        {
            if (_useTransaction)
            {
                switch (_transactionType)
                {
                    case TransactionTypes.DbTransaction:
                        if (_transaction != null && _transaction.Connection != null)
                        {
                            SaveChanges();
                            _transaction.Commit();
                        }
                        break;

                    case TransactionTypes.TransactionScope:
                        try
                        {
                            if(_transactionScope != null)
                                _transactionScope.Complete();
                        }
                        catch (Exception error)
                        {
                            if (_rethrowExceptions)
                            {
                                throw;
                            }
                            else
                            {
                                if (RepositoryBaseExceptionRaised != null)
                                {
                                    RepositoryBaseExceptionRaised(error);
                                }
                            }
                        }

                        break;
                }

                if (startNewTransaction)
                    StartTransaction();
            }
            else
            {
                SaveChanges();
            }
        }

        public Int32 Count<R>() where R : class
        {
            return Model.Set<R>()
                .Count();
        }

        public bool Delete<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        entry.State = System.Data.EntityState.Deleted;
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    Model.Set<R>().Remove(entity);

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            RepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }

            });

            return result;
        }

        public bool DeleteMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = Delete<R>(e));

            return result;
        }

        public bool DeleteMultipleWithCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e =>
            {
                result = Delete<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        public void Detach(object entity)
        {
            var objectContext = ((IObjectContextAdapter)Model).ObjectContext;
            var entry = Model.Entry(entity);

            if (entry.State != EntityState.Detached)
                objectContext.Detach(entity);
        }

        public void Detach(List<object> entities)
        {
            entities.ForEach(e => Detach(e));
        }

        public IQueryable<R> Find<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = default(IQueryable<R>);

            ProcessTransactionableMethod(() => {
                entities = SetEntities<R>().Where(where);
            });

            return entities;
        }

        public IQueryable<R> Find<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            ProcessTransactionableMethod(() => 
            { 
                if (includes != null)
                {
                    entities = ApplyIncludesToQuery<R>(entities, includes);
                }

                entities = entities.Where(where);
            });

            return entities;
        }

        public R First<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity =  entities
                    .First(where);
            });

            return entity;
        }

        public R FirstOrDefault<R>(Expression<Func<R, bool>> where, params Expression<Func<R, object>>[] includes) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                if (where != null)
                    entities = entities.Where(where);

                if (includes != null)
                {
                    entities = ApplyIncludesToQuery<R>(entities, includes);
                }

                entity = entities.FirstOrDefault();
            });

            return entity;
        }

        public IQueryable<R> GetAll<R>() where R : class
        {
            IQueryable<R> entities = default(IQueryable<R>);

            ProcessTransactionableMethod(() =>
            {
                entities = SetEntities<R>();
            });

            return entities;
        }

        public IQueryable<R> GetAll<R>(params Expression<Func<R, object>>[] includes) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            if (includes != null)
            {
                entities = ApplyIncludesToQuery<R>(entities, includes);
            }

            return entities;
        }

        public DbConnection GetConnection()
        {
            return _connection;
        }

        public void SaveChanges()
        {
            Model.SaveChanges();
        }

        public void SetIdentityCommand()
        {
            List<EntitySetBase> sets;

            var container =
                   ((IObjectContextAdapter)Model).ObjectContext.MetadataWorkspace
                      .GetEntityContainer(
                            ((IObjectContextAdapter)Model).ObjectContext.DefaultContainerName,
                            DataSpace.CSpace);

            sets = container.BaseEntitySets.ToList();

            foreach (EntitySetBase set in sets)
            {
                string command = string.Format("SET IDENTITY_INSERT {0} {1}", set.Name, "ON");
                ((IObjectContextAdapter)Model).ObjectContext.ExecuteStoreCommand(command);
            }
        }

        public void SetIsolationLevel(IsolationLevel isolationLevel)
        {
            _isolationLevel = isolationLevel;
        }

        public void SetRethrowExceptions(bool rehtrowExceptions)
        {
            _rethrowExceptions = rehtrowExceptions;
        }

        public void SetTransactionType(TransactionTypes transactionType)
        {
            _transactionType = transactionType;
        }

        public void SetUseTransaction(bool useTransaction)
        {
            _useTransaction = useTransaction;
        }

        public R Single<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .Single(where);
            });

            return entity;
        }

        public R SingleOrDefault<R>(Expression<Func<R, bool>> where) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .SingleOrDefault(where);
            });

            return entity;
        }

        public R SingleOrDefault<R>(Expression<Func<R, bool>> where, Expression<Func<R, object>> include) where R : class
        {
            IQueryable<R> entities = SetEntities<R>();

            R entity = default(R);

            ProcessTransactionableMethod(() =>
            {
                entity = entities
                    .Include(include)
                    .SingleOrDefault(where);
            });

            return entity;
        }

        public bool Update<R>(R entity) where R : class
        {
            bool result = false;

            ProcessTransactionableMethod(() =>
            {
                try
                {
                    var entry = SetEntry(entity);

                    if (entry != null)
                    {
                        entry.State = EntityState.Modified;
                    }
                    else
                    {
                        Model.Set<R>().Attach(entity);
                    }

                    SaveChanges();

                    result = true;
                }
                catch (Exception error)
                {
                    var entry = Model.Entry(entity);
                    entry.State = EntityState.Unchanged;

                    RollBack();
                    Detach(entity);

                    if (_rethrowExceptions)
                    {
                        throw;
                    }
                    else
                    {
                        if (RepositoryBaseExceptionRaised != null)
                        {
                            RepositoryBaseExceptionRaised(error);
                        }
                    }
                }
                finally
                { }
            });

            return result;
        }

        public bool UpdateMultiple<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => result = Update<R>(e));

            return result;
        }

        public bool UpdateMultipleWithCommit<R>(List<R> entities) where R : class
        {
            bool result = false;

            entities.ForEach(e => 
            {
                result = Update<R>(e);
                CommitTransaction(startNewTransaction: true);
            });

            return result;
        }

        internal IQueryable<R> ApplyIncludesToQuery<R>(IQueryable<R> entities, Expression<Func<R, object>>[] includes) where R : class
        {
            if (includes != null)
                entities = includes.Aggregate(entities, (current, include) => current.Include(include));

            return entities;
        }

        internal void ProcessTransactionableMethod(Action action)
        {
            StartTransaction();
            action();
        }

        internal IQueryable<R> SetEntities<R>() where R : class
        {
            IQueryable<R> entities = Model.Set<R>();

            return entities;
        }

        internal DbSet<R> SetEntity<R>() where R : class
        {
            DbSet<R> entity = Model.Set<R>();

            return entity;
        }

        internal DbEntityEntry SetEntry<R>(R entity) where R : class
        {
            DbEntityEntry entry = Model.Entry(entity);

            return entry;
        }

        internal IQueryable<T> GetQuery(Expression<Func<T, object>> include)
        {
            IQueryable<T> entities = SetEntities<T>()
                .Include(include);

            return entities;
        }

        internal void RollBack()
        {
            if (_useTransaction)
            {
                if (_transactionType == TransactionTypes.DbTransaction)
                {
                    if (_transaction != null && _transaction.Connection != null)
                    {
                        _transaction.Rollback();
                    }
                }
            }
        }

        internal void StartTransaction()
        {
            if (_useTransaction)
            {
                switch(_transactionType)
                {
                    case TransactionTypes.DbTransaction:
                        if (_transaction == null || _transaction.Connection == null)
                            _transaction = _connection.BeginTransaction(IsolationLevel.ReadUncommitted);
                        break;

                    case TransactionTypes.TransactionScope:
                        _transactionScope = new TransactionScope();
                        break;
                }
            }
        }

        public void Dispose()
        {
            CommitTransaction();

            if (!Object.Equals(Model, null))
            {
                Model.Dispose();
                Model = null;
            }

            if (_connection != null)
            {
                _connection.Close();
            }

            _transaction = null;
            _transactionScope = null;
            _connection = null;
        }
    }
}
