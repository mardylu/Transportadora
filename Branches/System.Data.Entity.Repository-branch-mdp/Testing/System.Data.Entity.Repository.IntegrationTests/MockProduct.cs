using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.Entity.Repository.Testing.Entities;

namespace System.Data.Entity.Repository.IntegrationTests
{
    public class MockProduct
    {
        public Product CreateSingleProduct()
        {
            Product product = new Product()
            {
                ProductName = "Duvel Green",
                SupplierID = 1,
                CategoryID = 1,
                QuantityPerUnit = "24 bottles per case",
                UnitPrice = (decimal)30.00,
                UnitsInStock = 12,
                UnitsOnOrder = 24,
                ReorderLevel = 12,
                Discontinued = false
            };

            return product;
        }

        public List<Product> CreateMultipleProducts(int count)
        {
            List<Product> products = new List<Product>();

            for (int i = 0; i < count; i++)
            {
                products.Add(new Product()
                {
                    ProductName = string.Format("Duvel Green {0}", i.ToString()),
                    SupplierID = 1,
                    CategoryID = 1,
                    QuantityPerUnit = "24 bottles per case",
                    UnitPrice = (decimal)30.00,
                    UnitsInStock = 12,
                    UnitsOnOrder = 24,
                    ReorderLevel = 12,
                    Discontinued = false
                });
            }

            return products;
        }
    }
}
