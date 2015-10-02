using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests.Repositories
{
    public class TransactionScopeRepository : RepositoryBase<RepositoryContext>, ITransactionScopeRepository
    {
        public TransactionScopeRepository()
            : base(throwExceptions: true, transactionType: TransactionTypes.TransactionScope)
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

        public bool AddProduct(Product product)
        {
            bool result = false;

            result = Add<Product>(product);

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
    }
}
