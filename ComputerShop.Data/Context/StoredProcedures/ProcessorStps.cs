using System.Data.Entity;
using ComputerShop.Data.Context.StoredProcedures.Base;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context.StoredProcedures
{
    public class ProcessorStps : SimpleResultBaseStps<Processor>
    {
        public ProcessorStps(DbContext context)
            : base(context)
        {
        }
    }
}