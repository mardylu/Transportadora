using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests.Repositories
{
    public interface ISupplierRepository : IRepositoryBase
    {
        Supplier FindSupplierBySupplierID(int supplierID);
    }
}
