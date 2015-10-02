using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity.Repository.Testing.Entities;
using System.Data.Entity.Repository.IntegrationTests.Repositories;
using System.Collections.Generic;

namespace System.Data.Entity.Repository.IntegrationTests
{
    [TestClass]
    public class TransactionAddEntitiesTests
    {
        private IProductRepository _productRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _productRepository = new ProductRepository();
        }

        [TestMethod]
        public void Transaction_AddEntity()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProduct(product);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddEntityWithoutTransaction()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProductWithoutTransaction(product);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddMultipleEntities()
        {
            bool result = false;

            List<Product> products = new MockProduct().CreateMultipleProducts(5000);

            result = _productRepository.AddMultipleProducts(products);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddMultipleEntitiesWithCommitPerAdd()
        {
            bool result = false;

            List<Product> products = new MockProduct().CreateMultipleProducts(5000);

            result = _productRepository.AddMultipleProductsWithCommit(products);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_AddEntityWithFind()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProduct(product);

            Product productResult = _productRepository.FindProductByProductName("Duvel Green");

            Assert.IsNotNull(productResult);
        }

        [TestMethod]
        public void Transaction_AddEntityTwiceWithFind()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProduct(product);

            Product productResult = _productRepository.FindProductByProductName("Duvel Green");

            result = _productRepository.AddProduct(product);

            Assert.IsNotNull(productResult);
        }

        [TestMethod]
        public void Transaction_AddWithoutCommitWithDeleteWithCommitAndUpdate()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProduct(product);
            result = _productRepository.DeleteProduct(product);

            product.ProductName = "Duvel Hop";
            result = _productRepository.UpdateProduct(product);

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Transaction_SimulateMultipleReadsAndUpdates()
        {
            throw new NotImplementedException();
        }

        [TestMethod]
        public void Transaction_AddEntityWithSingleTransactionAndDontCreateNewTransaction()
        {
            bool result = false;
            bool startNewTransaction = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProduct(product);
            _productRepository.CommitTransaction(startNewTransaction);

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _productRepository.Dispose();
        }
    }
}
