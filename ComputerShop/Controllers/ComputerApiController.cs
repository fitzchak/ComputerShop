using System.Linq;
using System.Net;
using System.Web.Http;
using ComputerShop.Data.Context;
using ComputerShop.Data.Model;

namespace ComputerShop.Controllers
{
    public class ComputerApiController : ApiController
    {
        protected ComputerRepository Repository { get; set; }

        public ComputerApiController()
        {
            Repository = new ComputerRepository(new ComputerShopContext());
        }

        public ComputerApiController(ComputerRepository repository)
        {
            Repository = repository;
        }

        public IQueryable<Computer> Get()
        {
            return Repository.Get();
        }

        public Computer Get(int id)
        {
            var result = Repository.GetByID(id);

            if (result == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return result;
        }
    }
}