using System;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ComputerShop.Data.Context.StoredProcedures.Base;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Context.StoredProcedures
{
    public class ComputerStps : SimpleResultBaseStps<Computer>
    {
        public ComputerStps(DbContext context) : base(context)
        {
        }
    }
}
