using BlazorDownloadFile;
using CsvHelper;
using Kr.Models;
using KR.API.Data;
using KR.Web.Services.Base;
using Radzen;
using System.Globalization;
using System.Text;

namespace KR.Web.Services
{
    public class ExportService : ServiceBase
    {
        private readonly BlazorDownloadFile.IBlazorDownloadFileService blazorDownloadFile;
        public ExportService(StoreDbContext storeDbContext, IBlazorDownloadFileService blazorDownloadFile) : base(storeDbContext)
        {
            this.blazorDownloadFile = blazorDownloadFile;
        }

        public void ImportUsersFromCvs(byte[] stream)
        {
            List<User> users = new List<User>();
            using (MemoryStream mem = new MemoryStream(stream))
            using (var reader = new StreamReader(mem))
            {
                using (var csv = new CsvReader(reader, CultureInfo.GetCultureInfo("ru")))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        User user = new User();
                        user = csv.GetRecord<User>();
                        users.Add(user);
                    }
                }
            }

            storeDbContext.AddRange(users);
            storeDbContext.SaveChanges();
            Reload();
        }

        public async Task ExportUsersToCsv(List<User> userEx)
        {
            List<User> users = userEx;
            using (MemoryStream mem = new MemoryStream())
            {
                using (var writer = new StreamWriter(mem, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("ru")))
                    {
                        csv.WriteRecords(users);
                    }
                }
                await blazorDownloadFile.DownloadFile("ExportUsers_" + DateTime.Now.ToShortDateString() + ".csv", mem.ToArray(), "application/ostet-stream");
            }
        }

        public async Task ExportProductsToCsv(List<ProductStorage> productsEx)
        {
            List<ProductStorage> products = productsEx;
            using (MemoryStream mem = new MemoryStream())
            {
                using (var writer = new StreamWriter(mem, Encoding.UTF8))
                {
                    using (var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("ru")))
                    {
                        csv.WriteRecords(products);
                    }
                }
                await blazorDownloadFile.DownloadFile("ExportProducts_" + DateTime.Now.ToShortDateString() + ".csv", mem.ToArray(), "application/ostet-stream");
            }
        }

        public void ImportProductsFromCvs(byte[] stream)
        {
            List<ProductStorage> products = new List<ProductStorage>();
            List<Category> categories = new List<Category>();
            using (MemoryStream mem = new MemoryStream(stream))
            using (var reader = new StreamReader(mem))
            {
                using (var csv = new CsvReader(reader, CultureInfo.GetCultureInfo("ru")))
                {
                    csv.Read();
                    csv.ReadHeader();
                    while (csv.Read())
                    {
                        Category category = new Category();
                        category = csv.GetRecord<Category>();
                        categories.Add(category);
                        ProductStorage product = new ProductStorage();
                        product = csv.GetRecord<ProductStorage>();
                        products.Add(product);
                    }
                }
            }
            categories = categories.GroupBy(x => x.Name).Select(x => x.First()).ToList();
            for (int i =0; i < categories.Count; i++)
            {
                if (storeDbContext.Categories.Where(x => x.Name == categories[i].Name).Any())
                {
                    categories[i] = storeDbContext.Categories.Where(x => x.Name == categories[i].Name).First();
                }
            }
            for (int i = 0; i < products.Count; i++)
            {
                for (int j = 0; j < categories.Count; j++)
                {
                    if (products[i].Category.Name == categories[j].Name)
                    {
                        products[i].Category = categories[j];
                        storeDbContext.Add(products[i]);
                        storeDbContext.SaveChanges();
                        break;
                    }
                }
            }
            Reload();
        }

    }
}
