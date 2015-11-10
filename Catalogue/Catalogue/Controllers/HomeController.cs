using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catalogue.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            ViewBag.Username = Session["Login"];

            Models.CatalogueDBEntities db = new Models.CatalogueDBEntities();

            foreach(Models.User user in db.Users)
            {
                if (string.Compare(user.UserName,Session["Login"].ToString()) == 0)
                    return View(user.Catalogues);
            }

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}