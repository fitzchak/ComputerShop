//using System.ComponentModel.DataAnnotations.Schema;

using System;
using System.Data.Entity;
//using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;
using ComputerShop.Data.Context.StoredProcedures;
using ComputerShop.Data.Context.StoredProcedures.Base;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context
{
    public class ComputerShopContext : DbContext
    {
        private ComputerStps _computerStps;
        private ProcessorStps _processorStps;
        private ComputerBrandStps _computerBrandStps;

        public DbSet<Computer> Computers { get; set; }

        public DbSet<ComputerBrand> CompBrands { get; set; }

        public DbSet<Processor> Processors { get; set; }

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
        
        public ProcessorStps ProcessorStps
        {
            get
            {
                if (_processorStps == null)
                {
                    _processorStps = new ProcessorStps(this);
                }
                return _processorStps;
            }
        }

        public ComputerBrandStps ComputerBrandStps
        {
            get
            {
                if (_computerBrandStps == null)
                {
                    _computerBrandStps = new ComputerBrandStps(this);
                }
                return _computerBrandStps;
            }
        }

        public void ExecuteStp<TEntity>(TEntity entity, SimpleResultBaseStps<TEntity> stps, BaseStps.StpEnum stp)
            where TEntity : class, IHaveId
        {
            switch (stp)
            {
                case BaseStps.StpEnum.Update:
                    stps.GetUpdateStp(entity).Execute(this);
                    break;
                case BaseStps.StpEnum.Insert:
                    stps.GetInsertStp(entity).Execute(this);
                    break;
                case BaseStps.StpEnum.Delete:
                    stps.GetDeleteStp(entity.Id).Execute(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException("stp");
            }
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();

            ConfigureComputer(modelBuilder);

            ConfigureComputerBrand(modelBuilder);

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
                .IsConcurrencyToken(true);

            //configuration
            //    .Property(t => t.Id)
            //    .HasColumnName("CompID");

            //configuration
            //    .HasRequired(m => m.ComputerBrand)
            //    .WithMany()
            //    .Map(m => m.MapKey("CompBrandID"))
            //    .WillCascadeOnDelete(false);

            //configuration
            //    .HasOptional(m => m.Processor)
            //    .WithMany()
            //    .WillCascadeOnDelete(false);

            configuration
                .Property(t => t.ComputerModel)
                .HasMaxLength(4000);

            configuration
                .Property(t => t.Description)
                .HasMaxLength(4000);
        }

        private void ConfigureComputerBrand(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<ComputerBrand>();

            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration.Property(m => m.Timestamp)
                          .IsConcurrencyToken(true);
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            //configuration.ToTable("CompBrand");

            //configuration
            //    .Property(t => t.Id)
            //    .HasColumnName("CompBrandID");

            configuration
                .Property(t => t.Name)
                .HasMaxLength(255);

            //configuration
            //    .Property(t => t.Name)
            //    .HasColumnName("CompBrandName");
        }

        private void ConfigureProcessor(DbModelBuilder modelBuilder)
        {
            var configuration = modelBuilder.Entity<Processor>();

            configuration
                .Property(t => t.Timestamp)
                .IsRowVersion();

            configuration
                .Property(m => m.Timestamp)
                .IsConcurrencyToken(true);
            //.HasDatabaseGeneratedOption(DatabaseGeneratedOption.Computed);

            configuration
                .Property(t => t.Name)
                .HasMaxLength(255);

            configuration
                .Property(t => t.Description)
                .HasMaxLength(4000);
        }
    }
}