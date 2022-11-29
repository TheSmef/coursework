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

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class PurchaseServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private PurchaseAgreementService PurchaseAgreementService;
        private ProductService ProductService;
        private CategoryService CategoryService;
        private PurchaseService PurchaseService;
        private StorageHistoryService StorageHistoryService;
        public PurchaseServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            PurchaseAgreementService = new PurchaseAgreementService(StoreDbContext);
            ProductService = new ProductService(StoreDbContext);
            CategoryService = new CategoryService(StoreDbContext);
            PurchaseService = new PurchaseService(StoreDbContext);
            StorageHistoryService = new StorageHistoryService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void AddPurchase_Passed()
        {
            try
            {
                PurchaseService.AddPurchase(new Purchase()
                {
                    ProductStorage = new ProductStorage() { Name = "Помидоры", Amount = 0, Category = new Category() { Name = "Овощи" }, Cost = 1, Exipiration_Date = 0 },
                    Amount = 1,
                    Date_Creation = DateTime.Now,
                    Date_Purchase = DateTime.Now,
                    Price = 12,
                    PurchaseAgreement = new PurchaseAgreement() { Agreement_Code = "qwerty", Date_Of_Purchase = DateTime.Now, Provider = "ОАО Помидор"}
                }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(2)]
        public void CheckPurchase_Existing_False()
        {
            Assert.False(PurchaseService.CheckPurchase(PurchaseService.GetPurchases().Result.First()));
        }

        [Fact]
        [TestPriority(3)]
        public void CheckUserPost_NonExisting_True()
        {
            Assert.True(PurchaseService.CheckPurchase(new Purchase()));
        }

        [Fact]
        [TestPriority(4)]
        public void EditPurchase_Passed()
        {
            try
            {
                Purchase purchase = PurchaseService.GetPurchases().Result.Where(x => x.ProductStorage.Name == "Помидоры").First();
                purchase.Amount = 10;
                purchase.Price = 100;
                purchase.Date_Creation = DateTime.Now;
                purchase.Date_Purchase = DateTime.Now;
                PurchaseService.EditPurchase(purchase).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(5)]
        public void GetPurchaseId_Passed()
        {
            try
            {
                Purchase purchase = PurchaseService.GetPurchases().Result.First();
                Assert.StrictEqual<Purchase>(purchase, PurchaseService.GetPurchaseById(purchase.Id_Puchase).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(6)]
        public void GetPurchases_Passed()
        {
            try
            {
                Assert.True(PurchaseService.GetPurchases().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(7)]
        public void DeletePurchase_True()
        {
            try
            {
                Assert.True(PurchaseService.DeletePurchase(PurchaseService.GetPurchases().Result.Where(x => x.ProductStorage.Name == "Помидоры").First()));
                PurchaseAgreementService.DeletePurchaseAgreement(PurchaseAgreementService.GetPurchaseAgreements().Result.Where(x => x.Agreement_Code == "qwerty").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Помидоры").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Помидоры").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Помидоры").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Помидоры").First());
                ProductService.DeleteProduct(ProductService.GetProducts().Result.Where(x => x.Name == "Помидоры").First());
                CategoryService.DeleteCategory(CategoryService.GetCategories().Result.Where(x => x.Name == "Овощи").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
