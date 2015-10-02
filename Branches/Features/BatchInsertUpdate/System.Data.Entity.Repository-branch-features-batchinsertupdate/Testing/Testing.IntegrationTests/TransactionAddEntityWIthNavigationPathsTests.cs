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
        private ISupplierRepository _supplierRepository;

        [TestInitialize]
        public void TestInitialize()
        {
            _productRepository = new ProductRepository();
            _supplierRepository = new SupplierRepository();
        }

        [TestMethod]
        public void Transaction_AddEntityWithNavigationPath()
        {
            bool result = false;

            Supplier supplier = _supplierRepository.FindSupplierBySupplierID(1);

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
