using System;
using System.Data.Entity;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Web.Http;
using Breeze.WebApi;
using ComputerShop.Data.Context;
using ComputerShop.Data.Context.StoredProcedures.Base;
using ComputerShop.Data.Model;
using ComputerShop.Data.Repository;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ComputerShop.Controllers
{
    [BreezeController]
    public class ComputerShopController : ApiController
    {
        protected EFContextProvider<ComputerShopContext> ContextProvider { get; private set; }
        protected ComputerShopContext ComputerShopContext { get; private set; }

        protected ComputerShopController()
        {
            ContextProvider = new EFContextProvider<ComputerShopContext>();
            ComputerShopContext = new ComputerShopContext();
        }

        [HttpGet]
        public virtual string Metadata()
        {
            return ContextProvider.Metadata();
        }

        [HttpGet]
        public object Lookups()
        {
            var processors = ContextProvider.Context.Processors;
            var computerBrands = ContextProvider.Context.CompBrands;

            var lookups = new { processors, computerBrands};
            return lookups;
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
            // for deleted should have "Deleted"
            var resultEntity = UseStps(saveBundle);

            if (resultEntity != null)
            {
                var result = new SaveResult()
                                 {
                                     Entities = new System.Collections.Generic.List<object>()
                                 };

                result.Entities.Add(resultEntity);

                return result;
            }

            return ContextProvider.SaveChanges(saveBundle);
        }

        private object UseStps(JObject saveBundle)
        {

            dynamic obj = JsonConvert.DeserializeObject(saveBundle.ToString());

            var currentEntity = obj.entities[0];
            var currentEntityString = currentEntity.ToString();

            string state = currentEntity.entityAspect.entityState.Value;

            // Computer:#ComputerShop.Data.Model
            var currentEntityLongType = (string)currentEntity.entityAspect.entityTypeName;
            string currentEntityType = currentEntityLongType.Split(':')[0];

            BaseStps.StpEnum? stp = null;

            switch (state)
            {
                case "Deleted":
                    // call delete stp for given type
                    stp = BaseStps.StpEnum.Delete;
                    break;
                case "Modified":
                    stp = BaseStps.StpEnum.Update;
                    break;
                default:
                    stp = null;
                    break;
            }

            if (stp == null )
            {
                return null;
            }

            switch (currentEntityType)
            {
                case "Computer":
                    var computer = JsonConvert.DeserializeObject<Computer>(currentEntityString);
                    ComputerShopContext.ExecuteStp<Computer>(computer, ComputerShopContext.ComputerStps, stp.Value);

                    return computer;
                case "ComputerBrand":
                    var computerBrand = JsonConvert.DeserializeObject<ComputerBrand>(currentEntityString);
                    ComputerShopContext.ExecuteStp<ComputerBrand>(computerBrand, ComputerShopContext.ComputerBrandStps, stp.Value);

                    return computerBrand;
                case "Processor":
                    var processor = JsonConvert.DeserializeObject<Processor>(currentEntityString);
                    ComputerShopContext.ExecuteStp<Processor>(processor, ComputerShopContext.ProcessorStps, stp.Value);

                    return processor;
                default:

                    return null;
            }

        }
    }
}