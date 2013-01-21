using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using ComputerShop.Data.Context;

namespace ComputerShop.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            var model = new ComputerRepository(new ComputerShopContext()).Get();

            return View(model);
        }

    }
}
