using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests.Repositories
{
    public interface ITransactionScopeRepository : IRepositoryBase
    {
        bool AddProduct(Product product);
        bool DeleteProduct(Product product);
        bool UpdateProduct(Product product);
        bool UpdateMultipleProducts(List<Product> products);

        Product FindProductByProductName(string productName);
        List<Product> FindProductsByProductName(string productName);

        Supplier FindSupplierBySupplierID(int supplierID);
    }
}
