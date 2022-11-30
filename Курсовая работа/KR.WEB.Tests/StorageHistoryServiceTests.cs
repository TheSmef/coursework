using Kr.Models;
using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class StorageHistoryServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private StorageHistoryService StorageHistoryService;
        private ProductService ProductService;
        private CategoryService CategoryService;
        public StorageHistoryServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            StorageHistoryService = new StorageHistoryService(StoreDbContext);
            ProductService = new ProductService(StoreDbContext);
            CategoryService = new CategoryService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void GetStorageHistory_Passed()
        {
            try
            {
                ProductService.AddProduct(new ProductStorage() { Amount = 0, Category = new Category() { Name = "Прочее"}, Exipiration_Date = 0, Cost = 12, Name = "Молоко" }).Wait(30000);
                Assert.True(StorageHistoryService.GetStorageHistory().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(2)]
        public void DeleteStorageHistory_True()
        {
            try
            {
                Assert.True(StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Молоко").First()));
                ProductService.DeleteProduct(ProductService.GetProducts().Result.Where(x => x.Name == "Молоко").First());
                CategoryService.DeleteCategory(CategoryService.GetCategories().Result.Where(x => x.Name == "Прочее").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}