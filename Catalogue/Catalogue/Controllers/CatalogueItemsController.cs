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
    public class CatalogueItemsController : Controller
    {
        private CatalogueDBEntities db = new CatalogueDBEntities();

        // GET: CatalogueItems
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            var catalogueItems = db.CatalogueItems.Include(c => c.Catalogue);
            return View(catalogueItems.ToList());
        }

        // GET: CatalogueItems/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogueItem catalogueItem = db.CatalogueItems.Find(id);
            if (catalogueItem == null)
            {
                return HttpNotFound();
            }
            return View(catalogueItem);
        }

        // GET: CatalogueItems/Create
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title");
            return View();
        }

        // POST: CatalogueItems/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Deadline,Description,DateCreated,DateModified,Complete")] CatalogueItem catalogueItem)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                catalogueItem.DateCreated = DateTime.Now;
                catalogueItem.DateModified = DateTime.Now;
                catalogueItem.CataID = int.Parse(Session["CataID"].ToString());
                catalogueItem.ItemID = db.CatalogueItems.Count<Models.CatalogueItem>() + 1;

                db.CatalogueItems.Add(catalogueItem);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title", catalogueItem.CataID);
            return View(catalogueItem);
        }

        // GET: CatalogueItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogueItem catalogueItem = db.CatalogueItems.Find(id);
            if (catalogueItem == null)
            {
                return HttpNotFound();
            }
            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title", catalogueItem.CataID);
            return View(catalogueItem);
        }

        // POST: CatalogueItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ItemID,CataID,Title,Deadline,Description,DateCreated,DateModified,Complete")] CatalogueItem catalogueItem)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                db.Entry(catalogueItem).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title", catalogueItem.CataID);
            return View(catalogueItem);
        }

        // GET: CatalogueItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            CatalogueItem catalogueItem = db.CatalogueItems.Find(id);
            if (catalogueItem == null)
            {
                return HttpNotFound();
            }
            return View(catalogueItem);
        }

        // POST: CatalogueItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            CatalogueItem catalogueItem = db.CatalogueItems.Find(id);
            db.CatalogueItems.Remove(catalogueItem);
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

        public ActionResult CataItemCreate(int cataID)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            Session["CataID"] = cataID;

            return RedirectToAction("Create");
        }
    }
}
