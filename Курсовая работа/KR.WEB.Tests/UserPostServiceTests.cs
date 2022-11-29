using Kr.Models;
using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class UserPostServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private UserPostService UserPostService;
        private UserService UserService;
        private PostService PostService;
        public UserPostServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            UserPostService = new UserPostService(StoreDbContext);
            UserService = new UserService(StoreDbContext);
            PostService = new PostService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void AddUserPost_Passed()
        {
            try
            {
                UserPostService.CreateUserPost(new UserPost() { User = UserService.GetUsers().Result.Where(x => x.Account.Login == "admin").First(), Post = new Post() { Name = "Продавец", Salary = 20000 }, Share = 1 }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(2)]
        public void CheckUserPost_Existing_False()
        {
            Assert.False(UserPostService.CheckUserPost(UserPostService.GetUserPosts().Result.First()));
        }

        [Fact]
        [TestPriority(3)]
        public void CheckUserPost_NonExisting_True()
        {
            Assert.True(UserPostService.CheckUserPost(new UserPost()));
        }

        [Fact]
        [TestPriority(4)]
        public void EditUserPost_Passed()
        {
            try
            {
                UserPost userpost = UserPostService.GetUserPosts().Result.Where(x => x.Post.Name == "Продавец").First();
                userpost.Share = (decimal)0.1;
                UserPostService.EditUserPost(userpost).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(5)]
        public void GetUserPostById_Passed()
        {
            try
            {
                UserPost userpost = UserPostService.GetUserPosts().Result.First();
                Assert.StrictEqual<UserPost>(userpost, UserPostService.GetUserPostById(userpost.Id_User_Post).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(6)]
        public void GetUserPostByIdToReadOnly_Passed()
        {
            try
            {
                UserPost userpost = UserPostService.GetUserPosts().Result.First();
                Assert.Equal(userpost.ToJson(), UserPostService.GetUserPostByIdToReadOnly(userpost.Id_User_Post).Result.ToJson());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(7)]
        public void GetUserPosts_Passed()
        {
            try
            {
                Assert.True(UserPostService.GetUserPosts().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(8)]
        public void DeleteUserPost_True()
        {
            try
            {
                Assert.True(UserPostService.DeleteUserPost(UserPostService.GetUserPosts().Result.Where(x => x.Post.Name == "Продавец").First()));
                PostService.DeletePost(PostService.GetPosts().Result.Where(x => x.Name == "Продавец").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
