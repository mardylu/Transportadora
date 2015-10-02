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
        private IProductRepository _productRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _productRepository = new ProductRepository();
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

            result = _productRepository.AddProduct(product);

            Product resultProduct = _productRepository.Find<Product>(x => x.ProductName == "Duvel Green")
                .FirstOrDefault();

            resultProduct.ProductName = "Duvel Red";

            result = _productRepository.Update<Product>(resultProduct);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Update a products navigation entity
        /// </summary>
        [TestMethod]
        public void Transaction_UpdateNavigationEntity()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            Supplier supplier = _productRepository.Find<Supplier>(x => x.SupplierID == 1)
                .FirstOrDefault();

            supplier.CompanyName = "Exotic oranges";
            product.Supplier = supplier;
            
            result = _productRepository.AddProduct(product);

            Product resultProduct = _productRepository.Find<Product>(x => x.ProductName == "Duvel Green")
                .FirstOrDefault();

            result = _productRepository.Update<Product>(resultProduct);

            Assert.IsTrue(result);
        }

        /// <summary>
        /// Update a products navigation entity
        /// </summary>
        [TestMethod]
        public void Transaction_UpdateNavigationEntityWithInclude()
        {
            bool result = false;

            Product product = new MockProduct()
                .CreateSingleProduct();

            result = _productRepository.AddProduct(product);

            Product resultProduct = _productRepository.Find<Product>(x => x.ProductName == "Duvel Green",
                x => x.Supplier)
                .FirstOrDefault();

            resultProduct.Supplier.CompanyName = "Exoctic liquids";

            result = _productRepository.Update<Product>(resultProduct);

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

            result = _productRepository.UpdateMultiple<Product>(products);

            List<Product> resultProducts = _productRepository.Find<Product>(x => x.ProductName == "Duvel Hop")
                .ToList();

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

            result = _productRepository.UpdateMultipleWithCommit<Product>(products);

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
