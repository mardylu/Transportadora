using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using System.Data.Entity.Repository.Testing.Entities;
using System.Data.Entity.Repository.IntegrationTests.Repositories;
using System.Collections.Generic;

namespace System.Data.Entity.Repository.IntegrationTests
{
    [TestClass]
    public class TransactionAddEntityWIthNavigationPathsTests
    {
        private IProductRepository _productRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _productRepository = new ProductRepository();
        }

        [TestMethod]
        public void Transaction_AddEntityWithNavigationPath()
        {
            bool result = false;

            Supplier supplier = _productRepository.Find<Supplier>(x => x.SupplierID == 1)
                .FirstOrDefault();

            Product product = new MockProduct()
                .CreateSingleProduct();

            product.Supplier = supplier;

            result = _productRepository.AddProduct(product);

            Assert.IsTrue(result);
        }
        
        [TestCleanup]
        public void TestCleanup()
        {
            _productRepository.Dispose();
        }
    }
}
