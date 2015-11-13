using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Security.Cryptography;
using System.IO;

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
                            SHA512 encryption = new SHA512Managed();
                            byte[] data = new byte[loginAccount.Password.Length];
                            byte[] result;
                            string temp = loginAccount.Password;

                            for (int i = 0; i < loginAccount.Password.Length; i++)
                                data[i] = (byte)loginAccount.Password[i];

                            result = encryption.ComputeHash(data);

                            string newString = "";

                            for (int i = 0; i < result.Length; i++)
                                newString += String.Format("{0:X2}", result[i]);

                            loginAccount.Password = newString;

                            if (string.Compare(user.Password, loginAccount.Password) == 0)
                            {
                                Session["Login"] = user.UserName;
                                Session["UserID"] = user.UserID;
                                return RedirectToAction("Index", "Home");
                            }

                            loginAccount.Password = temp;
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
                newUser.Email = accountToCreate.Email;
                newUser.UserID = userAccounts.Users.Count<Models.User>() + 1;

                SHA512 encryption = SHA512CryptoServiceProvider.Create();
                byte[] data = new byte[accountToCreate.Password.Length];
                byte[] result;

                for(int i = 0; i < accountToCreate.Password.Length; i++)
                    data[i] = (byte)accountToCreate.Password[i];

                result = encryption.ComputeHash(data);

                string newString = "";

                for (int i = 0; i < result.Length; i++)
                    newString += String.Format("{0:X2}", result[i]);

                newUser.Password = newString;

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