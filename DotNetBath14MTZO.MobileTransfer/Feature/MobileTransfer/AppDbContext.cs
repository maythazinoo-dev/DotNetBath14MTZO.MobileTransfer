using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace DotNetBath14MTZO.MobileTransfer.Feature.MobileTransfer
{
    public class AppDbContext : DbContext
    {
      private readonly SqlConnectionStringBuilder _sqlConnectionStringBuilder;
        public AppDbContext()
        {
            _sqlConnectionStringBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = ".",
                InitialCatalog = "MobileTransferAppDB",
                UserID = "sa",
                Password = "mtzoo@123",
                TrustServerCertificate = true
            };
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_sqlConnectionStringBuilder.ConnectionString);
            }
        }

        internal object GetMobileNo(string frommobileno)
        {
            throw new NotImplementedException();
        }

        public DbSet<UserModel> Tbl_UserModel{ get; set; }
        public DbSet<TransactionModel> Tbl_TransactionModel { get; set; }
    }
}
