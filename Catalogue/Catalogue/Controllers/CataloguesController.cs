using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Catalogue.Models;


namespace Catalogue.Controllers
{
    public class CataloguesController : Controller
    {
        private CatalogueDBEntities db = new CatalogueDBEntities();

        // GET: Catalogues
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            foreach (Models.User user in db.Users)
            {
                if (string.Compare(user.UserName, Session["Login"].ToString()) == 0)
                {
                    return View(user.Catalogues);
                }
            }

            return View();
        }

        // GET: Catalogues/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Catalogue catalogue = db.Catalogues.Find(id);
            if (catalogue == null)
            {
                return HttpNotFound();
            }
            return View(catalogue);
        }

        // GET: Catalogues/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            ViewBag.UserID = new SelectList(db.Users, "UserID", "UserName");
            return View();
        }

        // POST: Catalogues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CataID,UserID,Title,Priority,Description,DateCreated,DateModified")] Models.Catalogue catalogue)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                catalogue.CataID = db.Catalogues.Count<Models.Catalogue>() + 1;
                catalogue.UserID = int.Parse(Session["UserID"].ToString());
                catalogue.DateCreated = DateTime.Now;
                catalogue.DateModified = DateTime.Now;

                db.Catalogues.Add(catalogue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(catalogue);
        }

        // GET: Catalogues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Catalogue catalogue = db.Catalogues.Find(id);
            if (catalogue == null)
            {
                return HttpNotFound();
            }

            Session["CataID"] = catalogue.CataID;
            Session["UserID"] = catalogue.UserID;
            Session["DateCreated"] = catalogue.DateCreated;

            return View(catalogue);
        }

        // POST: Catalogues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CataID,UserID,Title,Priority,Description,DateCreated,DateModified")] Models.Catalogue catalogue)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {

                catalogue.CataID = int.Parse(Session["CataID"].ToString());
                catalogue.UserID = int.Parse(Session["UserID"].ToString());
                catalogue.DateCreated = DateTime.Parse(Session["DateCreated"].ToString());
                catalogue.DateModified = DateTime.Now;
                db.Entry(catalogue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(catalogue);
        }

        // GET: Catalogues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Models.Catalogue catalogue = db.Catalogues.Find(id);
            if (catalogue == null)
            {
                return HttpNotFound();
            }
            return View(catalogue);
        }

        // POST: Catalogues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            Models.Catalogue catalogue = db.Catalogues.Find(id);
            db.Catalogues.Remove(catalogue);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
