using Kr.Models;
using KR.API.Data;
using KR.Models;
using KR.Web.Models;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class AuthServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private AuthService AuthService;
        private UserService UserService;
        public AuthServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            AuthService = new AuthService(StoreDbContext);
            UserService = new UserService(StoreDbContext);
        }

        [Fact]
        [TestPriority(1)]
        public void PrepareAuth_Admin_True()
        {
            Assert.True(Guid.Empty != AuthService.PrepareAuth("admin",
                "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec"));
        }

        [Fact]
        [TestPriority(2)]
        public void PrepareAuth_NonExisting_False()
        {
            Assert.False(Guid.Empty != AuthService.PrepareAuth("13412341", "asdfadsf"));
        }

        [Fact]
        [TestPriority(3)]
        public void CreateNewAccount_True()
        {
            try
            {
                AuthService.CreateNewAccount(new RegistrationModel() { Last_name = "Соколов", First_name = "Александр", BirthDate = DateTime.Now, Email = "sokolov@gmail.com",
                    Password = "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec",
                    Login = "sokolov"});
                UserService.DeleteUser(UserService.GetUsers().Result.Where(x => x.Account.Login == "sokolov").First());

            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(4)]
        public void CheckEmailForUnique_NonExistingName_True()
        {
            Assert.True(AuthService.CheckEmailForUnique("123@123.sd"));
        }

        [Fact]
        [TestPriority(5)]
        public void CheckEmailForUnique_ExistingName_False()
        {
            Assert.False(AuthService.CheckEmailForUnique("admin@admin.admin"));
        }

        [Fact]
        [TestPriority(6)]
        public void CheckPostName_ExistingName_True()
        {
            Assert.True(AuthService.CheckLoginForUnique("123"));
        }

        [Fact]
        [TestPriority(7)]
        public void CheckLoginForUnique_ExistingName_False()
        {
            Assert.False(AuthService.CheckLoginForUnique("admin"));
        }


        [Fact]
        [TestPriority(8)]
        public void GetgetAccountByIdById_Passed()
        {
            try
            {
                Account? account = AuthService.getAccountById(Guid.Parse("1d9b1fc1-6273-4102-822b-7d4b92541216"));
                Assert.True(account != null);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

    }
}