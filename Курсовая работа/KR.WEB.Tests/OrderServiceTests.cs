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
    public class OrderServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private UserPostService UserPostService;
        private UserService UserService;
        private PostService PostService;
        private OrderService OrderService;
        public OrderServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            UserPostService = new UserPostService(StoreDbContext);
            UserService = new UserService(StoreDbContext);
            PostService = new PostService(StoreDbContext);
            OrderService = new OrderService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void AddOrder_Passed()
        {
            try
            {
                OrderService.AddOrder(new Order()
                {
                    Date_Order= DateTime.Now,
                    UserPost = new UserPost() { User = UserService.GetUsers().Result.Where(x => x.Account.Login == "admin").First(), Post = new Post() { Name = "Кассир", Salary = 15000 }, Share = 1 }
                }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(2)]
        public void EditOrder_Passed()
        {
            try
            {
                Order order = OrderService.GetOrders().Result.Where(x => x.UserPost.Post.Name == "Кассир").First();
                order.Date_Order= DateTime.Now;
                order.UserPost = new UserPost() { User = UserService.GetUsers().Result.Where(x => x.Account.Login == "admin").First(), Post = new Post() { Name = "Охранник", Salary = 25000 }, Share = 1 };
                OrderService.EditOrder(order).Wait(30000);
                UserPostService.DeleteUserPost(UserPostService.GetUserPosts().Result.Where(x => x.Post.Name == "Кассир").First());
                PostService.DeletePost(PostService.GetPosts().Result.Where(x => x.Name == "Кассир").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(3)]
        public void GetOrderById_Passed()
        {
            try
            {
                Order order = OrderService.GetOrders().Result.First();
                Assert.StrictEqual<Order>(order, OrderService.GetOrderById(order.Id_Order).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(4)]
        public void GetOrders_Passed()
        {
            try
            {
                Assert.True(OrderService.GetOrders().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(5)]
        public void DeleteOrder_True()
        {
            try
            {
                Assert.True(OrderService.DeleteOrder(OrderService.GetOrders().Result.Where(x => x.UserPost.Post.Name == "Охранник").First()));
                UserPostService.DeleteUserPost(UserPostService.GetUserPosts().Result.Where(x => x.Post.Name == "Охранник").First());
                PostService.DeletePost(PostService.GetPosts().Result.Where(x => x.Name == "Охранник").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
