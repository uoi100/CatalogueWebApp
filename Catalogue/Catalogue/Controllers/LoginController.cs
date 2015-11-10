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
            LoginAccount loginAccount = new LoginAccount();

            return View(loginAccount);
        }

        //
        // POST: Login
        [HttpPost]
        public ActionResult Index(LoginAccount loginAccount)
        {
            if (ModelState.IsValid)
            {
                // todo: If the loginAccount object is valid
                // Compare to see if the object exists in the database
                Catalogue.Models.CatalogueDBEntities database = new Models.CatalogueDBEntities();

                if (database.Database.Exists())
                {
                    foreach( Models.User user in database.Users)
                    {
                        if(string.Compare(user.UserName.ToLower(), loginAccount.UserName.ToLower()) == 0)
                        {
                            if(string.Compare(user.Password, loginAccount.Password) == 0)
                            {
                                Session["Login"] = user.UserName;
                                Session["UserID"] = user.UserID;
                                return RedirectToAction("Index", "Home");
                            }
                        }
                    }
                }
            }

            ViewBag.Error = "Invalid Login / Password";

            return View(loginAccount);
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
                // If the accountToCreate object is valid
                // we'll need to save it in a database
                Catalogue.Models.CatalogueDBEntities userAccounts = new Models.CatalogueDBEntities();
                userAccounts.Database.CreateIfNotExists();
                
                if (userAccounts.Users.Count<Models.User>() > 0)
                    foreach( Models.User user in userAccounts.Users)
                    {
                        if(string.Compare(accountToCreate.UserName.ToLower(), user.UserName.ToLower()) == 0)
                        {
                            ViewBag.Error = "Username already exists";

                            return View(accountToCreate);
                        }
                    }

                Models.User newUser = new Models.User();

                newUser.UserName = accountToCreate.UserName;
                newUser.Password = accountToCreate.Password;
                newUser.Email = accountToCreate.Email;
                newUser.UserID = userAccounts.Users.Count<Models.User>() + 1;

                userAccounts.Users.Add(newUser);
                userAccounts.SaveChanges();

                // After saving we'll redirect the user back to Login's Index page
                return Redirect("/");
            }

            // Invalid -- redisplay form with errors
            return View(accountToCreate);
        }
    }
}