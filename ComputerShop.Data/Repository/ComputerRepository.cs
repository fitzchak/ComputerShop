using ComputerShop.Data.Context;
using ComputerShop.Data.Model;

namespace ComputerShop.Data.Repository
{
    public class ComputerRepository : GenericRepository<ComputerShopContext, Computer>
    {
        public ComputerRepository(ComputerShopContext context) : base(context)
        {
        }
    }

    public class ComputerBrandRepository : GenericRepository<ComputerShopContext, ComputerBrand>
    {
        public ComputerBrandRepository(ComputerShopContext context) : base(context)
        {
        }
    }
}