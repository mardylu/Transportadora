using System;
using System.Linq;
using System.Data.Entity.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity.Repository.Testing.Entities;
using System.Data.Entity.Repository.IntegrationTests.Repositories;
using System.Collections.Generic;

namespace System.Data.Entity.Repository.IntegrationTests
{
    [TestClass]
    public class TransactionUpdateEntitiesTests
    {
        private ITransactionScopeRepository _transactionScopeRepository;

        [TestInitialize]
        public void TestInitialize()
        {

            _transactionScopeRepository = new TransactionScopeRepository();
        }

        [TestMethod]
        public void Transaction_AddEntity()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _transactionScopeRepository.AddProduct(product);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Add a Procuct and update value(s)
        /// </summary>
        [TestMethod]
        public void Transaction_UpdateEntity()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _transactionScopeRepository.AddProduct(product);

            Product resultProduct = _transactionScopeRepository.FindProductByProductName("Duvel Green");

            resultProduct.ProductName = "Duvel Red";
            result = _transactionScopeRepository.UpdateProduct(resultProduct);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Expected DBConcurrencyException because you cannot update entities that don't exists in DB. Use AddOrUpdate instead
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void Transaction_UpdateMultipleEntities()
        {
            bool result = false;

            List<Product> products = new MockProduct()
                .CreateMultipleProducts(5000);

            products.ForEach(x => x.ProductName = "Duvel Hop");

            result = _transactionScopeRepository.UpdateMultipleProducts(products);

            List<Product> resultProducts = _transactionScopeRepository.FindProductsByProductName("Duvel Hop");

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Expected DBConcurrencyException because you cannot update entities that don't exists in DB. Use AddOrUpdate instead
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void Transaction_UpdateMultipleEntitiesWithCommit()
        {
            bool result = false;

            List<Product> products = new MockProduct()
                .CreateMultipleProducts(5000);

            products.ForEach(x => x.ProductName = "Duvel Hop");

            result = _transactionScopeRepository.UpdateMultipleWithCommit(products);

            List<Product> resultProducts = _transactionScopeRepository.FindProductsByProductName("Duvel Hop");

            Assert.IsTrue(result);
        }

        [TestCleanup]
        public void TestCleanup()
        {
            _transactionScopeRepository.Dispose();
        }
    }
}
