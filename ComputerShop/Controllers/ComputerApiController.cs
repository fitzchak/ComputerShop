using System.Linq;
using System.Web.Http;
using Breeze.WebApi;
using ComputerShop.Data.Context;
using ComputerShop.Data.Model;
using ComputerShop.Data.Repository;

namespace ComputerShop.Controllers
{
    //[JsonFormatter, ODataActionFilter]
    //public class ComputerController : ComputerShopApiController<Computer>
    //{
    //    [HttpGet]
    //    public override IQueryable<Computer> Get()
    //    {
    //        return Repository.Context.Computers;
    //    }

    //    [HttpGet]
    //    public string Metadata()
    //    {
    //        return base.MetadataProtected();
    //    }
    //}

    //[JsonFormatter, ODataActionFilter]
    //public class ComputerBrandController : ComputerShopApiController<ComputerBrand>
    //{
    //    [HttpGet]
    //    public override IQueryable<ComputerBrand> Get()
    //    {
    //        return Repository.Context.CompBrands;
    //    }

    //    [HttpGet]
    //    public string Metadata()
    //    {
    //        return base.MetadataProtected();
    //    }
    //}
}