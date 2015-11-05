using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Catalogue.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            ViewBag.Title = "Login Page";
            ViewBag.Message = "Please Login";

            return View();
        }

        //
        // GET: /Login/Create
        public ActionResult Create()
        {
            UserAccount newAccount = new UserAccount();
            return View(newAccount);
        }

        //
        // POST: /Login/Create

            [HttpPost]
        public ActionResult Create(UserAccount accountToCreate)
        {
            if (ModelState.IsValid)
            {
                // Todo: If the accountToCreate object is valid
                // we'll need to save it in a database
                Models.User newUser = new Models.User();

                newUser.UserName = accountToCreate.UserName;
                newUser.Password = accountToCreate.Password;
                newUser.Email = accountToCreate.Email;

                Catalogue.Models.CatalogueDBEntities userAccounts = new Models.CatalogueDBEntities();
                userAccounts.Database.CreateIfNotExists();
                userAccounts.Users.Add(newUser);
                userAccounts.SaveChanges();

                // After saving we'll redirect the user to homepage
                return Redirect("/");
            }

            // Invalid -- redisplay form with errors
            return View(accountToCreate);
        }
    }
}