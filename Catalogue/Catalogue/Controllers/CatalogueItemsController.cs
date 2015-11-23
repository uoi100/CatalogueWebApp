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

            foreach (Models.User user in db.Users)
            {
                if (string.Compare(user.UserName, Session["Login"].ToString()) == 0)
                {
                    LinkedList<CatalogueItem> catalogueItems = new LinkedList<CatalogueItem>();
                    foreach(Models.Catalogue catalogue in user.Catalogues)
                    {
                        foreach (Models.CatalogueItem item in catalogue.CatalogueItems)
                            catalogueItems.AddLast(item);
                    }

                    return View(catalogueItems.ToList<CatalogueItem>());
                }
            }

            return View();
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
                
                if(Session["CataID"] != null)
                    catalogueItem.CataID = int.Parse(Session["CataID"].ToString());
                if (Session["SubCataID"] != null)
                    catalogueItem.SubCataID = int.Parse(Session["SubCataID"].ToString());

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

            Session["ItemID"] = catalogueItem.ItemID;
            Session["CataID"] = catalogueItem.CataID;
            Session["DateCreated"] = catalogueItem.DateCreated;
            return View(catalogueItem);
        }

        // POST: CatalogueItems/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Exclude = "ItemID,CataID,DateCreated,DateModified,Catalogue",Include = "Title,Deadline,Description,DateModified,Complete")] CatalogueItem catalogueItem)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            if (ModelState.IsValid)
            {
                catalogueItem.ItemID = int.Parse(Session["ItemID"].ToString());
                catalogueItem.CataID = int.Parse(Session["CataID"].ToString());
                catalogueItem.DateCreated = DateTime.Parse(Session["DateCreated"].ToString());
                catalogueItem.DateModified = DateTime.Now;
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

        public ActionResult SubCataItemCreate(int subCataID)
        {
            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            Session["SubCataID"] = subCataID;

            return RedirectToAction("Create");
        }

        public ActionResult CataItemEdit(int cataID, int itemID)
        {

            if (string.IsNullOrEmpty(Session["Login"] as string))
                return RedirectToAction("Index", "Login");

            Session["CataID"] = cataID;
            Session["ItemID"] = itemID;

            return RedirectToAction("Edit", "CatalogueItems", itemID);
        }
    }
}
