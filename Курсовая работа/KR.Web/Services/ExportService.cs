using CsvHelper;
using Kr.Models;
using KR.API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Collections;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Text;

namespace KR.Web.Services
{
    public class ExportService
    {
        private readonly StoreDbContext storeDbContext;
        public ExportService(StoreDbContext storeDbContext)
        {
            this.storeDbContext = storeDbContext;
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

        public async Task<byte[]> ExportUsersToCsv()
        {
            try
            {
                List<User> users = await storeDbContext.Users.Include(x => x.Account).ThenInclude(x => x.Roles).ToListAsync();
                using (MemoryStream mem = new MemoryStream())
                {
                    using (var writer = new StreamWriter(mem, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("ru")))
                        {
                            csv.WriteRecords(users);
                        }
                    }
                    
                    return mem.ToArray();
                }
            }
            catch
            {
                return null;
            }
        }

        public async Task<byte[]> ExportProductsToCsv()
        {
            try
            {
                List<ProductStorage> users = await storeDbContext.ProductStorages.Include(x => x.Category).ToListAsync();
                using (MemoryStream mem = new MemoryStream())
                {
                    using (var writer = new StreamWriter(mem, Encoding.UTF8))
                    {
                        using (var csv = new CsvWriter(writer, CultureInfo.GetCultureInfo("ru")))
                        {
                            csv.WriteRecords(users);
                        }
                    }

                    return mem.ToArray();
                }
            }
            catch
            {
                return null;
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

        public void Reload()
        {
            foreach (var entry in storeDbContext.ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Modified:
                        entry.State = EntityState.Unchanged;
                        break;
                    case EntityState.Deleted:
                        entry.Reload();
                        break;
                }
            }
        }
    }
}
