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

            result = _productRepository.AddOrUpdateProduct(product);

            Product resultProduct = _productRepository.FindProductByProductName("Duvel Green");

            resultProduct.ProductName = "Duvel Red";
            result = _productRepository.UpdateProduct(resultProduct);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddOrUpdateMultipleEntities()
        {
            bool result = false;

            List<Product> products = new MockProduct()
                .CreateMultipleProducts(5000);

            products.ForEach(x => x.ProductName = "Duvel Hop");

            result = _productRepository.AddOrUpdateMultipleProducts(products);

            List<Product> resultProducts = _productRepository.FindProductsByProductName("Duvel Hop");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddOrUpdateMultipleEntitiesWithCommit()
        {
            bool result = false;

            List<Product> products = new MockProduct()
                .CreateMultipleProducts(5000);

            products.ForEach(x => x.ProductName = "Duvel Hop");

            result = _productRepository.AddOrUpdateMultipleProductsWithCommit(products);

            List<Product> resultProducts = _productRepository.FindProductsByProductName("Duvel Hop");

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_UpdateEntityProperties()
        {
            bool result = false;

            List<Product> resultProducts = _productRepository.FindProductsByProductName("Duvel Green");

            Product product = resultProducts.Take(1).FirstOrDefault();

            result = _productRepository.UpdateProperty<Product>(product, x => x.ProductName, x => x.UnitsInStock);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _productRepository.Dispose();
        }
    }
}
