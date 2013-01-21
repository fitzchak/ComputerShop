using System;
using System.Collections.Generic;
using System.Data.Entity;
using ComputerShop.Data.Context.StoredProcedures;
using ComputerShop.Data.Context.StoredProcedures.Base;

namespace ComputerShop.Data.Context
{
    public class ComputerShopInitializer : DropCreateDatabaseIfModelChanges<ComputerShopContext>
    {
        protected override void Seed(ComputerShopContext context)
        {
            base.Seed(context);

            var operations = new ComputerShopInitializerOperations();
            operations.Seed(context);
        }
    }

    public class ComputerShopInitializerOperations// : DropCreateDatabaseIfModelChanges<ComputerShopContext>
    {
        public virtual void Seed(ComputerShopContext context)
        {
            SeedStoredProcedures(context);
            SeedEntities(context);
        }

        protected virtual void SeedStoredProcedures(ComputerShopContext context)
        {
            SeedComputerStps(context);
            SeedComputerBrandStps(context);
            SeedComputerModelStps(context);
            SeedProcessorStps(context);
        }

        protected virtual void SeedComputerStps(ComputerShopContext context)
        {
            ProcessCUDScript(context, new ComputerStps(context));
        }

        protected virtual void SeedComputerBrandStps(ComputerShopContext context)
        {
            ProcessCUDScript(context, new ComputerBrandStps(context));
        }

        protected virtual void SeedComputerModelStps(ComputerShopContext context)
        {
            ProcessCUDScript(context, new ComputerModelStps(context));
        }

        protected virtual void SeedProcessorStps(ComputerShopContext context)
        {
            ProcessCUDScript(context, new ProcessorStps(context));
        }

        private void ProcessCUDScript<TEntity>(ComputerShopContext context, SimpleResultBaseStps<TEntity> stps) 
            where TEntity : class
        {
            context.Database.ExecuteSqlCommand(stps.GetInsertStp(null).GetCreateScript());
            context.Database.ExecuteSqlCommand(stps.GetUpdateStp(null).GetCreateScript());
            context.Database.ExecuteSqlCommand(stps.GetDeleteStp(null).GetCreateScript());
        }

        protected virtual void SeedEntities(ComputerShopContext context)
        {

        }
    }
}
