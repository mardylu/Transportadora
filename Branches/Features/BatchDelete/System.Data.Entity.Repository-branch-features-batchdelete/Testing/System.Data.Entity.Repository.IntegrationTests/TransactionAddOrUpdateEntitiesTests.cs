using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity.Repository.Testing.Entities;
using System.Data.Entity.Repository.IntegrationTests.Repositories;
using System.Collections.Generic;

namespace System.Data.Entity.Repository.IntegrationTests
{
    [TestClass]
    public class TransactionAddOrUpdateEntitiesTests
    {
        private IProductRepository _productRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _productRepository = new ProductRepository();
        }

        [TestMethod]
        public void Transaction_AddOrUpdateEntity()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddOrUpdate<Product>(product);

            Product resultProduct = _productRepository.Find<Product>(x => x.ProductName == "Duvel Green")
                .FirstOrDefault();

            resultProduct.ProductName = "Duvel Red";

            result = _productRepository.Update<Product>(resultProduct);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddOrUpdateMultipleEntities()
        {
            bool result = false;

            List<Product> products = new MockProduct()
                .CreateMultipleProducts(5000);

            products.ForEach(x => x.ProductName = "Duvel Hop");

            result = _productRepository.AddOrUpdateMultiple<Product>(products);

            List<Product> resultProducts = _productRepository.Find<Product>(x => x.ProductName == "Duvel Hop")
                .ToList();

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddOrUpdateMultipleEntitiesWithCommit()
        {
            bool result = false;

            List<Product> products = new MockProduct()
                .CreateMultipleProducts(5000);

            products.ForEach(x => x.ProductName = "Duvel Hop");

            result = _productRepository.AddOrUpdateMultipleCommit<Product>(products);

            List<Product> resultProducts = _productRepository.Find<Product>(x => x.ProductName == "Duvel Hop")
                .ToList();

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _productRepository.Dispose();
        }
    }
}
