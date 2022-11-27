using KR.API.Data;
using Microsoft.Data.SqlClient;
using BlazorDownloadFile;
using KR.Web.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace KR.Web.Services
{
    public class BackupService : ServiceBase
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly string _backupFolderFullPath = AppDomain.CurrentDomain.BaseDirectory + "Backup\\backup.bak";
        private readonly string _directory = AppDomain.CurrentDomain.BaseDirectory + "Backup";
        private readonly IBlazorDownloadFileService blazorDownloadFile;



        public BackupService(IConfiguration configuration, IBlazorDownloadFileService blazorDownloadFile, StoreDbContext storeDbContext) : base(storeDbContext)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("StoreBaseConncetion");
            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
            this.blazorDownloadFile = blazorDownloadFile;
        }


        public async Task BackupDatabase()
        {
            if (!File.Exists(_backupFolderFullPath))
            {
                File.Create(_backupFolderFullPath).Close();
            }
            using (var connection = new SqlConnection(_connectionString))
                {
                    var query = String.Format("BACKUP DATABASE [Store] TO DISK='{0}' WITH INIT, FORMAT", _backupFolderFullPath);

                    using (var command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    using (FileStream reader = new FileStream(_backupFolderFullPath, FileMode.Open))
                    {
                        await blazorDownloadFile.DownloadFile("BackUpStore_" + DateTime.Now.ToShortDateString() + ".bak", reader, "application/ostet-stream");
                    }
                }
        }

        public async Task RestoreBatabase(byte[] stream)
        {
            if (!File.Exists(_backupFolderFullPath))
            {
                File.Create(_backupFolderFullPath).Close();
            }
            using (MemoryStream mem = new MemoryStream(stream))
            {
                await File.WriteAllBytesAsync(_backupFolderFullPath, mem.ToArray());
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = String.Format("USE MASTER RESTORE DATABASE [Store] FROM DISK='{0}' WITH REPLACE", _backupFolderFullPath);
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
            var entitiesList = storeDbContext.ChangeTracker.Entries().ToList();
            foreach (var entity in entitiesList)
            {
                entity.State = EntityState.Detached;
            }
        }
    }
}
