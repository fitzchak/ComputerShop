using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context
{
    public class ComputerBrandsContext : DbContext
    {
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ConfigureComputerBrandConfiguration(modelBuilder);
        }

        private static void ConfigureComputerBrandConfiguration(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<ComputerBrand>();

            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration.ToTable("CompBrand");

            configuration
                .Property(t => t.Id)
                .HasColumnName("CompBrandID");

            configuration.Property(m => m.Timestamp)
                         .IsConcurrencyToken(true)
                         .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);
            configuration
                .Property(t => t.Name)
                .HasColumnName("CompBrandName");
        }

    }
}