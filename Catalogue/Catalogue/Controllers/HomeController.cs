using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Globalization;

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
                if (string.Compare(user.UserName, Session["Login"].ToString()) == 0) {
                    var catalogues = user.Catalogues;
                    catalogues.OrderBy(s => s.Priority);

                    int currentDay = DateTime.Today.Day;
                    int monthDays = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                    DateTime day = DateTime.Today;

                    string calendar = "";

                    for (var i = 0; i < monthDays - currentDay; i++) {
                        DateTime date = new DateTime(day.Year, day.Month, currentDay + i);
                        calendar += makeCalendar(date, user);
                    }

                    ViewBag.Calendar = calendar;

                    return View(catalogues.ToList());
                }
            }

            return View();
        }

        private string makeCalendar(DateTime time, Models.User user)
        {
            string newString = "<div class='calendarHeader'>";
            DateTimeFormatInfo dateFormat = CultureInfo.CurrentCulture.DateTimeFormat;

            newString +=  dateFormat.GetMonthName(time.Month) + " " + time.Day + "(" + dateFormat.GetAbbreviatedDayName(time.DayOfWeek) + "), " + time.Year + "<br/>";

            bool foundItems = false;

            Models.CatalogueDBEntities db = new Models.CatalogueDBEntities();

            var catalogues = user.Catalogues;

            

            foreach (Models.Catalogue cata in catalogues)
            {
                var catalogueItems = cata.CatalogueItems;

                foreach (Models.CatalogueItem item in catalogueItems)
                {
                    if (item.Deadline.Year == time.Year)
                        if (item.Deadline.Month == time.Month)
                            if (item.Deadline.Day == time.Day)
                            {
                                newString += "<div class='calendarItem'>";
                                newString += item.Title + "</div><br/>";
                                foundItems = true;
                            }
                }

                var subcatalogues = cata.SubCatalogues;

                foreach(Models.SubCatalogue subCata in subcatalogues)
                {
                    var subCataItems = subCata.CatalogueItems;

                    foreach (Models.CatalogueItem item in subCataItems)
                    {
                        if (item.Deadline.Year == time.Year)
                            if (item.Deadline.Month == time.Month)
                                if (item.Deadline.Day == time.Day)
                                {
                                    newString += "<div class='calendarItem'>";
                                    newString += item.Title + "</div><br/>";
                                    foundItems = true;
                                }
                    }
                }

                
            }

            if (foundItems)
                newString += "</div>";
            else
                return "";

            return newString;
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

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("index", "Login");
        }
    }
}