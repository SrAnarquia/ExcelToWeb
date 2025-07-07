using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExcelToWeb.Models;
using Newtonsoft.Json;

namespace ExcelToWeb.Controllers
{
    public class userInteractionItemsController : Controller
    {

        //CLASS VARIABLES
        private DateTime timeToday = DateTime.Now; //private to only use it in this class
        private DateTime timeOlder;

        //THIS IS MODEL
        private Supply_ChainEntities1 db = new Supply_ChainEntities1();

        // GET: userInteractionItems
        public ActionResult Index(DateTime? startDate = null, DateTime? endDate = null, string departmentFilter = "")
        {
            // Definir rangos para filtrar (si no vienen, tomamos todo)
            var start = startDate ?? DateTime.MinValue;
            var end = endDate?.Date.AddDays(1).AddTicks(-1) ?? DateTime.MaxValue; // hasta fin de día

            // Query base, filtrando fechas y departamento si se especifica
            var query = db.userInteractionItems
                .Where(u => u.Date >= start && u.Date <= end);

            if (!string.IsNullOrEmpty(departmentFilter))
                query = query.Where(u => u.Department == departmentFilter);

            // --- Top 5 Productos por interacciones
            var topProducts = query
                .GroupBy(u => u.Product)
                .Select(g => new {
                    Product = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();

            ViewBag.TopProductsJson = JsonConvert.SerializeObject(topProducts);

            // --- Top 5 Categorías por interacciones
            var topCategories = query
                .GroupBy(u => u.Category)
                .Select(g => new {
                    Category = g.Key,
                    Count = g.Count()
                })
                .OrderByDescending(x => x.Count)
                .Take(5)
                .ToList();
            

            ViewBag.TopCategoriesJson = JsonConvert.SerializeObject(topCategories);

            return View();
        }

        // GET: userInteractionItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInteractionItems userInteractionItems = db.userInteractionItems.Find(id);
            if (userInteractionItems == null)
            {
                return HttpNotFound();
            }
            return View(userInteractionItems);
        }

        // GET: userInteractionItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: userInteractionItems/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Product,Category,Date,Month,Hour,Department,IP,URL")] userInteractionItems userInteractionItems)
        {
            if (ModelState.IsValid)
            {
                db.userInteractionItems.Add(userInteractionItems);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(userInteractionItems);
        }

        // GET: userInteractionItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInteractionItems userInteractionItems = db.userInteractionItems.Find(id);
            if (userInteractionItems == null)
            {
                return HttpNotFound();
            }
            return View(userInteractionItems);
        }

        // POST: userInteractionItems/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Product,Category,Date,Month,Hour,Department,IP,URL")] userInteractionItems userInteractionItems)
        {
            if (ModelState.IsValid)
            {
                db.Entry(userInteractionItems).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(userInteractionItems);
        }

        // GET: userInteractionItems/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            userInteractionItems userInteractionItems = db.userInteractionItems.Find(id);
            if (userInteractionItems == null)
            {
                return HttpNotFound();
            }
            return View(userInteractionItems);
        }

        // POST: userInteractionItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            userInteractionItems userInteractionItems = db.userInteractionItems.Find(id);
            db.userInteractionItems.Remove(userInteractionItems);
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


        //Method added and created
        private DateTime lastDateMonth(DateTime actualDate)
        {

            //WE CHARGE INITIAL DATE ON DATETIME
            actualDate = DateTime.Now;

            //THIS IS THE RETURING VARIABLE
            DateTime lastDate;
            //VARIBALES FOR CALCULATION
            int year;
            int lastMonth;
            int days;
            int time;
            int minutes;
            string lastMonthString;

            //WE SEPARATE YEARS 
            year = actualDate.Year;
            lastMonth = actualDate.Month - 1;
            days = actualDate.Day;
            time = actualDate.Hour;
            minutes = actualDate.Minute;
            //USING STRING TO CREATE DATE 
            lastMonthString = ($"{year}/{lastMonth}/{days}" + " " + $"{time}:{minutes}");

            //WE CREATE WITH THIS SEPARATED DATA THE NEW MONTH
            lastDate = Convert.ToDateTime(lastMonthString);

            return lastDate;
        }










    }
}
