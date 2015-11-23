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
    public class SubCataloguesController : Controller
    {
        private CatalogueDBEntities db = new CatalogueDBEntities();

        // GET: SubCatalogues
        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            foreach (Models.User user in db.Users)
            {
                if (string.Compare(user.UserName, Session["Login"].ToString()) == 0)
                {
                    LinkedList<SubCatalogue> subCatalogues = new LinkedList<SubCatalogue>();
                    foreach (Models.Catalogue catalogue in user.Catalogues)
                    {
                        foreach (Models.SubCatalogue subCata in catalogue.SubCatalogues)
                            subCatalogues.AddLast(subCata);
                    }

                    return View(subCatalogues.ToList<SubCatalogue>());
                }
            }

            return View();
        }

        // GET: SubCatalogues/Details/5
        public ActionResult Details(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCatalogue subCatalogue = db.SubCatalogues.Find(id);
            if (subCatalogue == null)
            {
                return HttpNotFound();
            }
            return View(subCatalogue);
        }

        // GET: SubCatalogues/Create
        public ActionResult Create()
        {
            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title");
            return View();
        }

        // POST: SubCatalogues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CataID,Title,Priority,Description")] SubCatalogue subCatalogue)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                subCatalogue.DateCreated = DateTime.Now;
                subCatalogue.DateModified = DateTime.Now;
                subCatalogue.CataID = int.Parse(Session["CataID"].ToString());
                subCatalogue.SubCataId = db.SubCatalogues.Count<Models.SubCatalogue>() + 1;

                db.SubCatalogues.Add(subCatalogue);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title", subCatalogue.CataID);
            return View(subCatalogue);
        }

        // GET: SubCatalogues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCatalogue subCatalogue = db.SubCatalogues.Find(id);
            if (subCatalogue == null)
            {
                return HttpNotFound();
            }

            Session["CataID"] = subCatalogue.CataID;
            Session["SubCataID"] = subCatalogue.SubCataId;
            Session["DateCreated"] = subCatalogue.DateCreated;

            return View(subCatalogue);
        }

        // POST: SubCatalogues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubCataId,CataID,Title,Priority,Description,DateCreated,DateModified")] SubCatalogue subCatalogue)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                subCatalogue.CataID = int.Parse(Session["CataID"].ToString());
                subCatalogue.SubCataId = int.Parse(Session["SubCataID"].ToString());
                subCatalogue.DateCreated = DateTime.Parse(Session["DateCreated"].ToString());
                subCatalogue.DateModified = DateTime.Now;

                db.Entry(subCatalogue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CataID = new SelectList(db.Catalogues, "CataID", "Title", subCatalogue.CataID);
            return View(subCatalogue);
        }

        // GET: SubCatalogues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubCatalogue subCatalogue = db.SubCatalogues.Find(id);
            if (subCatalogue == null)
            {
                return HttpNotFound();
            }
            return View(subCatalogue);
        }

        // POST: SubCatalogues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubCatalogue subCatalogue = db.SubCatalogues.Find(id);
            db.SubCatalogues.Remove(subCatalogue);
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

        public ActionResult SubCataCreate(int cataID)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            Session["CataID"] = cataID;

            return RedirectToAction("Create");
        }

        public ActionResult SubCataEdit(int cataID, int subCataID)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            Session["CataID"] = cataID;
            Session["SubCataID"] = subCataID;

            return RedirectToAction("Edit");
        }
    }
}
