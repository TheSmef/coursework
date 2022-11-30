using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class StatsServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private StatsService StatsService;
        public StatsServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            StatsService = new StatsService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void GetStats_Passed()
        {
            try
            {
                Assert.True(StatsService.GetStats(DateTime.Now, DateTime.Now).Result.Count == 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
        [Fact]
        [TestPriority(2)]
        public void GetBuhStats_Passed()
        {
            try
            {
                Assert.True(StatsService.GetBuhStats(DateTime.Now, DateTime.Now).Result.Count == 3);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
