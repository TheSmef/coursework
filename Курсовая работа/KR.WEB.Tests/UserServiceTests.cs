using Kr.Models;
using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class UserServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private AuthService AuthService;
        private UserService UserService;
        public UserServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            AuthService = new AuthService(StoreDbContext);
            UserService = new UserService(StoreDbContext);
        }
    }

}
