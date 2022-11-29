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
    public class OrderProductServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private UserPostService UserPostService;
        private OrderProductService OrderProductService;
        private UserService UserService;
        private StorageHistoryService StorageHistoryService;
        private PostService PostService;
        private OrderService OrderService;
        private CategoryService CategoryService;
        private ProductService ProductService;
        public OrderProductServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            UserPostService = new UserPostService(StoreDbContext);
            UserService = new UserService(StoreDbContext);
            CategoryService = new CategoryService(StoreDbContext);
            PostService = new PostService(StoreDbContext);
            OrderService = new OrderService(StoreDbContext);
            OrderProductService = new OrderProductService(StoreDbContext);
            StorageHistoryService = new StorageHistoryService(StoreDbContext);
            ProductService = new ProductService(StoreDbContext);
        }


        [Fact]
        [TestPriority(1)]
        public void AddOrderProduct_Passed()
        {
            try
            {
                Order order = new Order()
                {
                    Date_Order = DateTime.Now,
                    UserPost = new UserPost() { User = UserService.GetUsers().Result.Where(x => x.Account.Login == "admin").First(), Post = new Post() { Name = "Менеджер по продажам", Salary = 15000 }, Share = 1 },
                };
                OrderService.AddOrder(order).Wait(30000);
                OrderProductService.AddOrderProduct(new OrderProduct()
                {
                    Product = new ProductStorage() { Category = new Category() { Name = "Детские товары" }, Amount = 10, Cost = 120, Exipiration_Date = 0, Name = "Игрушка" },
                    Order = order,
                    Amount = 10,
                    Price = 1200,
                }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.InnerException.Message);
            }
        }

        [Fact]
        [TestPriority(2)]
        public void CheckOrderProduct_Existing_False()
        {
            Assert.False(OrderProductService.CheckOrderProduct(OrderProductService.GetOrderProducts().Result.First()));
        }

        [Fact]
        [TestPriority(3)]
        public void CheckOrderProduct_NonExisting_True()
        {
            Assert.True(OrderProductService.CheckOrderProduct(new OrderProduct()));
        }

        [Fact]
        [TestPriority(4)]
        public void EditOrderProduct_Passed()
        {
            try
            {
                OrderProduct orderProduct = OrderProductService.GetOrderProducts().Result.Where(x => x.Order.UserPost.Post.Name == "Менеджер по продажам").First();
                orderProduct.Price = 600;
                orderProduct.Amount = 5;
                OrderProductService.EditOrderProduct(orderProduct).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(5)]
        public void GetOrderById_Passed()
        {
            try
            {
                OrderProduct order = OrderProductService.GetOrderProducts().Result.First();
                Assert.StrictEqual<OrderProduct>(order, OrderProductService.GetOrderProductById(order.Id_order_product).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(6)]
        public void GetOrderProducts_Passed()
        {
            try
            {
                Assert.True(OrderProductService.GetOrderProducts().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }


        [Fact]
        [TestPriority(7)]
        public void DeleteOrderProduct_True()
        {
            try
            {
                Assert.True(OrderProductService.DeleteOrderProduct(OrderProductService.GetOrderProducts().Result.Where(x => x.Order.UserPost.Post.Name == "Менеджер по продажам").First()));
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Игрушка").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Игрушка").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Игрушка").First());
                StorageHistoryService.DeleteStorageHistory(StorageHistoryService.GetStorageHistory().Result.Where(x => x.ProductStorage.Name == "Игрушка").First());
                Assert.True(ProductService.DeleteProduct(ProductService.GetProducts().Result.Where(x => x.Name == "Игрушка").First()));
                CategoryService.DeleteCategory(CategoryService.GetCategories().Result.Where(x => x.Name == "Детские товары").First());
                OrderService.DeleteOrder(OrderService.GetOrders().Result.Where(x => x.UserPost.Post.Name == "Менеджер по продажам").First());
                UserPostService.DeleteUserPost(UserPostService.GetUserPosts().Result.Where(x => x.Post.Name == "Менеджер по продажам").First());
                PostService.DeletePost(PostService.GetPosts().Result.Where(x => x.Name == "Менеджер по продажам").First());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}
