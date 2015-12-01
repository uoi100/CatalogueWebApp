using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DayPilot.Web.Mvc;
using DayPilot.Web.Mvc.Enums;
using DayPilot.Web.Mvc.Events.Calendar;
using Catalogue.Models;

namespace Catalogue.Controllers
{
    public class CalendarController : Controller
    {
        //
        // GET: /Backend/

        public ActionResult Backend()
        {
            return new Dpc().CallBack(this);
        }


        class Dpc : DayPilotCalendar
        {
            private CatalogueDBEntities db = new CatalogueDBEntities();

            protected override void OnInit(InitArgs e)
            {
                Update();
            }

            protected override void OnFinish()
            {
                if (UpdateType == CallBackUpdateType.None)
                {
                    return;
                }

                DataIdField = "ItemID";
                DataStartField = "Deadline";
                DataEndField = "Deadline";
                DataTextField = "Title";

                Events = from e in db.CatalogueItems select e;
            }
        }
    }
}
