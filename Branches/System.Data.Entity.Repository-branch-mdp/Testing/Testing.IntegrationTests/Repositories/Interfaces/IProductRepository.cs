using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests.Repositories
{
    public interface IProductRepository : IRepositoryBase
    {
        bool AddProduct(Product product);
        bool AddProductWithoutTransaction(Product product);
        bool AddMultipleProducts(List<Product> products);
        bool AddMultipleProductsWithCommit(List<Product> products);

        bool AddOrUpdateProduct(Product product);
        bool AddOrUpdateMultipleProducts(List<Product> products);
        bool AddOrUpdateMultipleProductsWithCommit(List<Product> products);

        bool DeleteProduct(Product product);

        bool UpdateProduct(Product product);
        bool UpdateMultipleProducts(List<Product> products);

        Product FindProductByProductName(string productName);
        List<Product> FindProductsByProductName(string productName);

        Product FindProductWithSupplierByProductName(string productName);
    }
}
