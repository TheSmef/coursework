using Kr.Models;
using KR.API.Data;
using KR.Web.Services;
using KR.WEB.Tests.Orderer;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;

namespace KR.WEB.Tests
{
    [TestCaseOrderer("KR.WEB.Tests.Orderer.PriorityOrderer", "KR.WEB.Tests")]
    public class CategoryServiceTests
    {
        private DbContextOptionsBuilder<StoreDbContext> options = new DbContextOptionsBuilder<StoreDbContext>();
        private StoreDbContext StoreDbContext;
        private CategoryService CategoryService;
        public CategoryServiceTests()
        {
            StoreDbContext = new StoreDbContext(options.UseSqlServer("Server=localhost; Database=Store; Trusted_Connection=True").Options);
            CategoryService = new CategoryService(StoreDbContext);
        }

        [Fact]
        [TestPriority(1)]
        public void CheckCategoryName_NonExistingName_True()
        {
            Assert.True(CategoryService.CheckCategoryName("123456789012345678901234567890123456789012345678901234567890"));
        }

        [Fact]
        [TestPriority(2)]
        public void AddCategory_Passed()
        {
            try
            {
                CategoryService.AddCategory(new Category() { Name = "Хозяйственные товары" }).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(3)]
        public void CheckCategoryName_ExistingName_False()
        {
            Assert.False(CategoryService.CheckCategoryName("Хозяйственные товары"));
        }

        [Fact]
        [TestPriority(4)]
        public void EditCategory_Passed()
        {
            try
            {
                Category category = CategoryService.GetCategories().Result.Where(x => x.Name == "Хозяйственные товары").First();
                category.Name = "Молочная продукция";
                CategoryService.EditCategory(category).Wait(30000);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(5)]
        public void GetCategoryById_Passed()
        {
            try
            {
                Category category = CategoryService.GetCategories().Result.First();
                Assert.StrictEqual<Category>(category, CategoryService.GetCategoryById(category.Id_Category).Result);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(6)]
        public void GetCategories_Passed()
        {
            try
            {
                Assert.True(CategoryService.GetCategories().Result.Count() > 0);
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(7)]
        public void GetCategoryByIdForReadOnly_Passed()
        {
            try
            {
                Category category = CategoryService.GetCategories().Result.AsNoTracking().First();
                Assert.Equal(category.ToJson(), CategoryService.GetCategoryByIdForReadOnly(category.Id_Category).Result.ToJson());
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }

        [Fact]
        [TestPriority(8)]
        public void DeleteCategory_True()
        {
            try
            {
                Assert.True(CategoryService.DeleteCategory(CategoryService.GetCategories().Result.Where(x => x.Name == "Молочная продукция").First()));
            }
            catch (Exception e)
            {
                Assert.Fail(e.Message);
            }
        }
    }
}