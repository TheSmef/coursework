using Kr.Models;
using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class PostServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private PostService PostService;
        public PostServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            PostService = new PostService(StoreDbContext);
        }

        [Fact]
        [TestPriority(1)]
        public void CheckPostName_NonExistingName_True()
        {

            Assert.True(PostService.CheckPostName("123456789012345678901234567890123456789012345678901234567890"));
        }

        [Fact]
        [TestPriority(2)]
        public void AddPost_Passed()
        {
            try
            {
                PostService.AddPost(new Post() { Name = "Менеджер", Salary = 70000 }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(3)]
        public void CheckPostName_ExistingName_False()
        {
            Assert.False(PostService.CheckPostName("Менеджер"));
        }

        [Fact]
        [TestPriority(4)]
        public void EditPost_Passed()
        {
            try
            {
                Post post = PostService.GetPosts().Result.Where(x => x.Name == "Менеджер").First();
                post.Name = "Уборщик";
                post.Salary = 20000;
                PostService.EditPost(post).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(5)]
        public void GetPostsById_Passed()
        {
            try
            {
                Post post = PostService.GetPosts().Result.First();
                Assert.StrictEqual<Post>(post, PostService.GetPostById(post.Id_Post).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(6)]
        public void GetPost_Passed()
        {
            try
            {
                Assert.True(PostService.GetPosts().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(7)]
        public void GetPostByIdForReadOnly_Passed()
        {
            try
            {
                Post post = PostService.GetPosts().Result.AsNoTracking().First();
                Assert.Equal(post.ToJson(), PostService.GetPostById(post.Id_Post).Result.ToJson());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(8)]
        public void DeletePost_True()
        {
            try
            {
                Assert.True(PostService.DeletePost(PostService.GetPosts().Result.Where(x => x.Name == "Уборщик").First()));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}