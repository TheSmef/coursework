using Kr.Models;
using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class PurchaseAgreementServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private PurchaseAgreementService PurchaseAgreementService;
        public PurchaseAgreementServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            PurchaseAgreementService = new PurchaseAgreementService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void AddPurchaseAgreement_Passed()
        {
            try
            {
                PurchaseAgreementService.AddPurchaseAgreement(new PurchaseAgreement() { Provider = "ОАО ооо", Agreement_Code = "1234567890", Date_Of_Purchase = DateTime.Now}).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(2)]
        public void EditPurchaseAgreement_Passed()
        {
            try
            {
                PurchaseAgreement purchaseAgreement = PurchaseAgreementService.GetPurchaseAgreements().Result.Where(x => x.Agreement_Code == "1234567890").First();
                purchaseAgreement.Provider = "ООО оао";
                purchaseAgreement.Date_Of_Purchase = DateTime.Now;
                purchaseAgreement.Agreement_Code = "0987654321";
                PurchaseAgreementService.EditPurchaseAgreement(purchaseAgreement).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(3)]
        public void GetPurchaseAgreementById_Passed()
        {
            try
            {
                PurchaseAgreement purchaseAgreement = PurchaseAgreementService.GetPurchaseAgreements().Result.First();
                Assert.StrictEqual<PurchaseAgreement>(purchaseAgreement, PurchaseAgreementService.GetPurchaseAgreementById(purchaseAgreement.Id_Purchase_Agreement).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(4)]
        public void GetPurchaseAgreements_Passed()
        {
            try
            {
                Assert.True(PurchaseAgreementService.GetPurchaseAgreements().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(5)]
        public void DeletePurchaseAgreement_True()
        {
            try
            {
                Assert.True(PurchaseAgreementService.DeletePurchaseAgreement(PurchaseAgreementService.GetPurchaseAgreements().Result.Where(x => x.Agreement_Code == "0987654321").First()));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}