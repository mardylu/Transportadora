using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests.Repositories
{
    public class ProductRepository : RepositoryBase<RepositoryContext>, IProductRepository
    {
        public ProductRepository()
            : base(throwExceptions: true, useTransactions: true)
        {
            base.RepositoryBaseExceptionRaised += ProductRepository_RepositoryBaseExceptionRaised;
        }

        void ProductRepository_RepositoryBaseExceptionRaised(Exception exception)
        {
            throw new NotImplementedException();
        }

        public bool AddProduct(Product product)
        {
            bool result = false;

            result = Add<Product>(product);

            return result;
        }

        public bool AddProductWithoutTransaction(Product product)
        {
            SetUseTransaction(false);

            bool result = false;

            result = Add<Product>(product);

            return result;
        }

        public bool AddMultipleProducts(List<Product> products)
        {
            bool result = false;

            foreach(Product product in products)
            {
                result = Add<Product>(product);
            }

            return result;
        }

        public bool AddMultipleProductsWithCommit(List<Product> products)
        {
            bool result = false;

            foreach(Product product in products)
            {
                result = Add<Product>(product);
                CommitTransaction(startNewTransaction: true);
            }

            return result;
        }

        public bool AddOrUpdateProduct(Product product)
        {
            bool result = false;

            result = AddOrUpdate<Product>(product);

            return result;
        }

        public bool AddOrUpdateMultipleProducts(List<Product> products)
        {
            bool result = false;

            foreach (Product product in products)
            {
                result = AddOrUpdateProduct(product);
            }

            return result;
        }

        public bool AddOrUpdateMultipleProductsWithCommit(List<Product> products)
        {
            bool result = false;
            
            foreach (Product product in products)
            {
                result = AddOrUpdateProduct(product);
                CommitTransaction(startNewTransaction: true);
            }

            return result;
        }

        public bool DeleteProduct(Product product)
        {
            bool result = false;

            result = Delete<Product>(product);

            return result;
        }

        public bool UpdateProduct(Product product)
        {
            bool result = false;

            result = Update<Product>(product);

            return result;
        }

        public bool UpdateMultipleProducts(List<Product> products)
        {
            bool result = false;

            foreach (Product product in products)
            {
                result = UpdateProduct(product);
            }

            return result;
        }

        public bool UpdateMultipleProductsWithCommit(List<Product> products)
        {
            bool result = false;

            foreach (Product product in products)
            {
                result = UpdateProduct(product);
                CommitTransaction(startNewTransaction: true);
            }

            return result;
        }

        public Product FindProductByProductName(string productName)
        {
            Product result = Find<Product>(x => x.ProductName == productName)
                .FirstOrDefault();

            return result;
        }

        public List<Product> FindProductsByProductName(string productName)
        {
            List<Product> result = Find<Product>(x => x.ProductName == productName)
                .ToList();

            return result;
        }

        public Product FindProductWithSupplierByProductName(string productName)
        {
            Product result = Find<Product>(x => x.ProductName == productName,
                x => x.Supplier)
                .FirstOrDefault();

            return result;
        }

    }
}
