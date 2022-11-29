using Kr.Models;
using KR.API.Data;
using KR.Web.Models;
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
        private UserService UserService;
        public UserServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            UserService = new UserService(StoreDbContext);
        }

        [Fact]
        [TestPriority(1)]
        public void CheckUserEmail_NonExistingEmail_True()
        {
            Assert.True(UserService.CheckUserEmail("admin@admin.ru"));
        }

        [Fact]
        [TestPriority(2)]
        public void CheckUserEmail_ExistingEmail_False()
        {
            Assert.False(UserService.CheckUserEmail("admin@admin.admin"));
        }

        [Fact]
        [TestPriority(3)]
        public void CheckUserLogin_ExistingLogin_False()
        {
            Assert.False(UserService.CheckUserLogin("admin"));
        }

        [Fact]
        [TestPriority(4)]
        public void CheckUserLogin_NonExistingLogin_True()
        {
            Assert.True(UserService.CheckUserLogin("admin12312"));
        }

        [Fact]
        [TestPriority(5)]
        public void AddUser_Passed()
        {
            try
            {
                Assert.True(UserService.CreateNewAccount(new RegistrationModel()
                {
                    Last_name = "Сорокин",
                    First_name = "Дмитрий",
                    Otch = "Владимирович",
                    BirthDate = DateTime.Now,
                    Email = "sorokin@gmail.com",
                    Password = "c7ad44cbad762a5da0a452f9e854fdc1e0e7a52a38015f23f3eab1d80b931dd472634dfac71cd34ebc35d16ab7fb8a90c81f975113d6c7538dc69dd8de9077ec",
                    Login = "sorokin",
                    Roles = new List<Role>() { new Role() { Name = "Отдел продаж"} }
                }));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(6)]
        public void EditUser_Passed()
        {
            try
            {
                User user = UserService.GetUsers().Result.Where(x => x.Account.Login == "sorokin").First();
                user.Last_name = "Иванов";
                user.First_name = "Иван";
                user.Otch = null;
                user.Account.Email = "ivan@inav.ru";
                user.Account.Login = "ivan";
                user.Account.Roles = new List<Role>() { new Role() { Name = "Отдел закупок" } };
                user.BirthDate = DateTime.Now;
                UserService.EditUser(user).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(7)]
        public void GetUserById_Passed()
        {
            try
            {
                User user = UserService.GetUsers().Result.Where(x => x.Account.Login == "ivan").First();
                Assert.StrictEqual<User>(user, UserService.GetUserById(user.Id_User).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(8)]
        public void GetUserByIdToReadOnly_Passed()
        {
            try
            {
                User user = UserService.GetUsers().Result.Where(x => x.Account.Login == "ivan").First();
                User userToReadOnly = UserService.GetUserByIdToReadOnly(user.Id_User).Result;
                user.Account.User = null;
                userToReadOnly.Account.User = null;
                user.Account.Roles = null;
                userToReadOnly.Account.Roles = null;
                Assert.Equal(user.ToJson(), userToReadOnly.ToJson());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(9)]
        public void GetUsers_Passed()
        {
            try
            {
                Assert.True(UserService.GetUsers().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(10)]
        public void DeleteUser_True()
        {
            try
            {
                Assert.True(UserService.DeleteUser(UserService.GetUsers().Result.Where(x => x.Account.Login == "ivan").First()));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }

}
