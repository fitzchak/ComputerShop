using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using ComputerShop.Data.Context.StoredProcedures;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context
{
    public class ComputerRepository : GenericRepository<ComputerShopContext, Computer>
    {
        public ComputerRepository(ComputerShopContext context) : base(context)
        {
        }
    }

    public class ComputerShopContext : DbContext
    {
        private ComputerStps _computerStps;
        public DbSet<Computer> Computers { get; set; }

        public DbSet<ComputerBrand> CompBrands { get; set; }

        public ComputerStps ComputerStps
        {
            get
            {
                if (_computerStps == null)
                {
                    _computerStps = new ComputerStps(this);
                }
                return _computerStps;
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ConfigureComputer(modelBuilder);

            ConfigureComputerBrand(modelBuilder);

            ConfigureComputerModel(modelBuilder);

            ConfigureProcessor(modelBuilder);
        }

        private void ConfigureComputer(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<Computer>();

            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration
                .Property(m => m.Timestamp)
                .IsConcurrencyToken(true)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
            configuration
                .Property(t => t.Id)
                .HasColumnName("CompID");

            configuration
                .HasRequired(m => m.ComputerBrand)
                .WithMany()
                .Map(m => m.MapKey("CompBrandID"))
                .WillCascadeOnDelete(false);
        }

        private void ConfigureComputerBrand(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<ComputerBrand>();
            
            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration.Property(m => m.Timestamp)
                          .IsConcurrencyToken(true)
                          .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            
            configuration.ToTable("CompBrand");
            
            configuration
                .Property(t => t.Id)
                .HasColumnName("CompBrandID");

            configuration
                .Property(t => t.Name)
                .HasMaxLength(255);

            configuration
                .Property(t => t.Name)
                .HasColumnName("CompBrandName");
        }

        private void ConfigureComputerModel(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<ComputerModel>();

            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration
                .Property(m => m.Timestamp)
                .IsConcurrencyToken(true)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            configuration
                .Property(t => t.Name)
                .HasMaxLength(255);
        }

        private void ConfigureProcessor(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<Processor>();

            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration
                .Property(m => m.Timestamp)
                .IsConcurrencyToken(true)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            configuration
                .Property(t => t.Name)
                .HasMaxLength(255);

            configuration
                .Property(t => t.Description)
                .HasMaxLength(4000);
        }
    }
}