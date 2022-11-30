using KR.API.Data;
using Kr.Models;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class SalaryHistoryServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private SalaryHistoryService SalaryHistoryService;
        private UserPostService UserPostService;
        private UserService UserService;
        private PostService PostService;
        public SalaryHistoryServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            UserPostService = new UserPostService(StoreDbContext);
            UserService = new UserService(StoreDbContext);
            PostService = new PostService(StoreDbContext);
            SalaryHistoryService = new SalaryHistoryService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void AddSalaryHistory_Passed()
        {
            try
            {
                SalaryHistoryService.AddSalaryHistory(new SalaryHistory() { UserPost = new UserPost() { User = UserService.GetUsers().Result.Where(x => x.Account.Login == "admin").First(), Post = new Post() { Name = "Директор", Salary = 20000 },
                    Share = 1 }, Payment = (decimal)10000, Premium = true, Date = DateTime.Now }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(2)]
        public void EditSalaryHistory_Passed()
        {
            try
            {
                SalaryHistory salary = SalaryHistoryService.GetSalaryHistorys().Result.Where(x => x.UserPost.Post.Name == "Директор").First();
                salary.Payment = (decimal)100;
                salary.Date = DateTime.Now;
                salary.Premium = false;
                SalaryHistoryService.EditSalaryHistory(salary).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(3)]
        public void GetSalaryHistoryById_Passed()
        {
            try
            {
                SalaryHistory salary = SalaryHistoryService.GetSalaryHistorys().Result.First();
                Assert.StrictEqual<SalaryHistory>(salary, SalaryHistoryService.GetSalaryHistoryById(salary.Id_SalaryHistory).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(4)]
        public void GetSalaryHistorys_Passed()
        {
            try
            {
                Assert.True(SalaryHistoryService.GetSalaryHistorys().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(5)]
        public void DeleteSalaryHistory_True()
        {
            try
            {
                Assert.True(SalaryHistoryService.DeleteSalaryHistory(SalaryHistoryService.GetSalaryHistorys().Result.Where(x => x.UserPost.Post.Name == "Директор").First()));
                UserPostService.DeleteUserPost(UserPostService.GetUserPosts().Result.Where(x => x.Post.Name == "Директор").First());
                PostService.DeletePost(PostService.GetPosts().Result.Where(x => x.Name == "Директор").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
