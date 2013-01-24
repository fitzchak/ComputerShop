using System.Linq;
using System.Net;
using System.Web.Http;
using Breeze.WebApi;
using ComputerShop.Data.Context;
using ComputerShop.Data.Model;
using ComputerShop.Data.Repository;

namespace ComputerShop.Controllers
{
    [BreezeController]
    public class ComputerShopController : ApiController 
    {
        protected EFContextProvider<ComputerShopContext> Repository { get; private set; }

        protected ComputerShopController()
        {
            Repository = new EFContextProvider<ComputerShopContext>();
        }

        [HttpGet]
        public virtual string Metadata()
        {
            return Repository.Metadata();
        }

        [HttpGet]
        public IQueryable<Computer> Computers()
        {
            return Repository.Context.Computers;
        }

        [HttpGet]
        public IQueryable<ComputerBrand> ComputerBrands()
        {
            return Repository.Context.CompBrands;
        }

        [HttpGet]
        public IQueryable<Processor> Processors()
        {
            return Repository.Context.Processors;
        }
    }
}