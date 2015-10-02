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
            : base(throwExceptions: true)
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

        public Product FindProductByName(string productName)
        {
            Product result = Find<Product>(x => x.ProductName == productName)
                .FirstOrDefault();

            return result;
        }
    }
}
