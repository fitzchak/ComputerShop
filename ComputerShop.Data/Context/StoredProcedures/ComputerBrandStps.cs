using System.Data.Entity;
using ComputerShop.Data.Context.StoredProcedures.Base;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context.StoredProcedures
{
    public class ComputerBrandStps : SimpleResultBaseStps<ComputerBrand>
    {
        public ComputerBrandStps(DbContext context)
            : base(context)
        {
        }
    }
}