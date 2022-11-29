using KR.API.Data;
using Kr.Models;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NuGet.Protocol;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class ProductServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private StorageHistoryService StorageHistoryService;
        private CategoryService CategoryService;
        private ProductService ProductService;
        public ProductServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            CategoryService = new CategoryService(StoreDbContext);
            StorageHistoryService = new StorageHistoryService(StoreDbContext);
            ProductService = new ProductService(StoreDbContext);
        }

        [Fact]
        [TestPriority(1)]
        public void CheckProductName_NonExistingName_True()
        {
            Assert.True(ProductService.CheckProductName("123456789012345678901234567890123456789012345678901234567890"));
        }

        [Fact]
        [TestPriority(2)]
        public void AddProduct_Passed()
        {
            try
            {
                ProductService.AddProduct(new ProductStorage() { Category = new Category() { Name = "Товары" }, Amount = 0, Cost = 12, Exipiration_Date = 0, Name = "Бананы" }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(3)]
        public void CheckCategoryName_ExistingName_False()
        {
            Assert.False(ProductService.CheckProductName("Бананы"));
        }

        [Fact]
        [TestPriority(4)]
        public void EditProduct_Passed()
        {
            try
            {
                ProductStorage product = ProductService.GetProducts().Result.Where(x => x.Name == "Бананы").First();
                product.Name = "Молоч";
                product.Amount = 12;
                product.Cost = 3;
                product.Exipiration_Date = 16;
                product.Category= new Category() { Name = "Молоч"};
                ProductService.EditProduct(product).Wait(30000);
                CategoryService.DeleteCategory(CategoryService.GetCategories().Result.Where(x => x.Name == "Товары").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(5)]
        public void GetProductById_Passed()
        {
            try
            {
                ProductStorage product = ProductService.GetProducts().Result.First();
                Assert.StrictEqual<ProductStorage>(product, ProductService.GetProductById(product.Id_Product_Storage).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(6)]
        public void GetProducts_Passed()
        {
            try
            {
                Assert.True(ProductService.GetProducts().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(7)]
        public void GetProductByIdForReadOnly_Passed()
        {
            try
            {
                ProductStorage product = ProductService.GetProducts().Result.AsNoTracking().First();
                Assert.Equal(product.ToJson(), ProductService.GetProductByIdToReadOnly(product.Id_Product_Storage).Result.ToJson());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(8)]
        public void DeleteProduct_True()
        {
            try
            {
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Молоч").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Молоч").First());
                Assert.True(ProductService.DeleteProduct(ProductService.GetProducts().Result.Where(x => x.Name == "Молоч").First()));
                CategoryService.DeleteCategory(CategoryService.GetCategories().Result.Where(x => x.Name == "Молоч").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
