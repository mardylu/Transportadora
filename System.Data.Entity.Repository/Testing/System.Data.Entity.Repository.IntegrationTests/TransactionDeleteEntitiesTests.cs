using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity.Repository.Testing.Entities;
using System.Data.Entity.Repository.IntegrationTests.Repositories;
using System.Collections.Generic;

namespace System.Data.Entity.Repository.IntegrationTests
{
    [TestClass]
    public class TransactionDeleteEntitiesTests
    {
        private IProductRepository _productRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _productRepository = new ProductRepository();
        }

        [TestMethod]
        public void Transaction_DeleteEntity()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddOrUpdate<Product>(product);

            Product resultProduct = _productRepository.Find<Product>(x => x.ProductName == product.ProductName)
                .FirstOrDefault();

            result = _productRepository.Delete<Product>(resultProduct);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_DeleteMultipleEntities()
        {
            bool result = false;

            //List<Product> products = new MockProduct()
               // .CreateMultipleProducts(5000);

            //result = _productRepository.AddOrUpdateMultiple<Product>(products);

            List<Product> resultProducts = _productRepository.Find<Product>(x => x.ProductName.Contains("Duvel Green"))
                .ToList();

            result = _productRepository.DeleteMultipleWithCommit<Product>(resultProducts);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _productRepository.Dispose();
        }
    }
}
