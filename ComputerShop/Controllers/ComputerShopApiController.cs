using System.Linq;
using System.Net;
using System.Web.Http;
using Breeze.WebApi;
using ComputerShop.Data.Context;
using ComputerShop.Data.Model;
using ComputerShop.Data.Repository;
using Newtonsoft.Json.Linq;

namespace ComputerShop.Controllers
{
    [BreezeController]
    public class ComputerShopController : ApiController 
    {
        protected EFContextProvider<ComputerShopContext> ContextProvider { get; private set; }

        protected ComputerShopController()
        {
            ContextProvider = new EFContextProvider<ComputerShopContext>();
        }

        [HttpGet]
        public virtual string Metadata()
        {
            return ContextProvider.Metadata();
        }

        [HttpGet]
        public IQueryable<Computer> Computers()
        {
            return ContextProvider.Context.Computers;
        }

        [HttpGet]
        public IQueryable<ComputerBrand> ComputerBrands()
        {
            return ContextProvider.Context.CompBrands;
        }

        [HttpGet]
        public IQueryable<Processor> Processors()
        {
            return ContextProvider.Context.Processors;
        }

        [HttpPost]
        public SaveResult SaveChanges(JObject saveBundle)
        {
            return ContextProvider.SaveChanges(saveBundle);
        }
    }
}