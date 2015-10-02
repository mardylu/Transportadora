using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests.Repositories
{
    public class SupplierRepository : RepositoryBase<RepositoryContext>, ISupplierRepository
    {
        public SupplierRepository()
            : base(throwExceptions: true)
        {
            base.RepositoryBaseExceptionRaised += SupplierRepository_RepositoryBaseExceptionRaised;
        }

        void SupplierRepository_RepositoryBaseExceptionRaised(Exception exception)
        {
            throw new NotImplementedException();
        }

        public Supplier FindSupplierBySupplierID(int supplierID)
        {
            Supplier result = Find<Supplier>(x => x.SupplierID == supplierID)
                .FirstOrDefault();

            return result;
        }
    }
}
