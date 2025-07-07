using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ExcelToWeb.Models;

namespace ExcelToWeb.Controllers
{
    public class inventoryItemsController : Controller
    {
        private Supply_ChainEntities1 db = new Supply_ChainEntities1();

        // GET: inventoryItems
        public ActionResult Index()
        {



            return View(db.inventoryItems.ToList());
        }

        // GET: inventoryItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inventoryItems inventoryItems = db.inventoryItems.Find(id);
            if (inventoryItems == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItems);
        }

        // GET: inventoryItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: inventoryItems/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,ItemName,ItemDescription,ItemPrice,ItemImage,CategoryName,DepartmentName,ProductImage,Stock,IsActive,CreatedAt")] inventoryItems inventoryItems)
        {
            if (ModelState.IsValid)
            {
                db.inventoryItems.Add(inventoryItems);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(inventoryItems);
        }

        // GET: inventoryItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inventoryItems inventoryItems = db.inventoryItems.Find(id);
            if (inventoryItems == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItems);
        }

        // POST: inventoryItems/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ItemName,ItemDescription,ItemPrice,ItemImage,CategoryName,DepartmentName,ProductImage,Stock,IsActive,CreatedAt")] inventoryItems inventoryItems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(inventoryItems).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(inventoryItems);
        }

        // GET: inventoryItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            inventoryItems inventoryItems = db.inventoryItems.Find(id);
            if (inventoryItems == null)
            {
                return HttpNotFound();
            }
            return View(inventoryItems);
        }

        // POST: inventoryItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            inventoryItems inventoryItems = db.inventoryItems.Find(id);
            db.inventoryItems.Remove(inventoryItems);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Items(int page = 1, int pageSize = 6)
        {
            var query = db.inventoryItems
                          .OrderBy(i => i.ItemName);

            int totalItems = query.Count();

            var items = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            return View(items);
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
