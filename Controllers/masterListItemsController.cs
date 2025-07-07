using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ExcelToWeb.Models;
using ExcelToWeb.Models.ViewModels;

namespace ExcelToWeb.Controllers
{
    public class masterListItemsController : Controller
    {
        private Supply_ChainEntities1 db = new Supply_ChainEntities1();

        //[Authorize]
        // GET: masterListItems
        public ActionResult Index(int page=1,int? Id=null, DateTime? filterFromDate=null, DateTime? filterToDate=null)
        {
            //Total data Iqueyable
            IQueryable<masterListItems> query=db.masterListItems;

            //Tes 0: Just for bringing data lower tha id 200 so this will bring 199 data items
            query = query.Where(r => r.Id <200);


            //paginacion
            int pageSize = 12;
            int totalItems = query.Count();

            var data = query
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);


            return View(data);
        }

        // GET: masterListItems/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            masterListItems masterListItems = db.masterListItems.Find(id);
            if (masterListItems == null)
            {
                return HttpNotFound();
            }
            return View(masterListItems);
        }

        // GET: masterListItems/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: masterListItems/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que quiere enlazarse. Para obtener 
        // más detalles, vea https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Type,DaysForShipReal,DaysForShipmentScheduled,BenefitPerOrder,SalesPerCustomer,DeliveryStatus,LateDelivery,CategoryId,CategoryName,CustomerCity,CustomerCountry,CustomerEmail,CustomerFName,CustomerId,CustomerLName,CustomerPassword,CustomerSegment,CustomerState,CustomerStreet,CustomerZipCode,DepartmentId,DepartmentName,Latitude,Longtude,Market,OrderCity,OrderCountry,OrderCustomerId,OrderDateDateOrders,OrderId,OrderCardpordId,OrderItemDiscount,OrderDiscountRate,OrderItemId,OrderItemProductPrice,OrderItemProfitRatio,OrderItemQuantity,Sales,OrderItemTotal,OrderProfitPerOrder,OrderRegion,OrderState,OrderStatus,OrderZipCode,ProductCardId,ProductCategoryId,ProductDescription,ProductImage,ProductName,ProductPrice,ProductStatus,ShippingDateDateOrders,ShippingMode")] masterListItems masterListItems)
        {
            if (ModelState.IsValid)
            {
                db.masterListItems.Add(masterListItems);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(masterListItems);
        }

        // GET: masterListItems/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            masterListItems masterListItems = db.masterListItems.Find(id);
            if (masterListItems == null)
            {
                return HttpNotFound();
            }
            return View(masterListItems);
        }


        // POST: masterListItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            masterListItems masterListItems = db.masterListItems.Find(id);
            db.masterListItems.Remove(masterListItems);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        //Metodo agregado para checar la informacion total de un comprador y crear analisis dentro de la pagina
        [HttpGet]
        public ActionResult Information(string Id) 
        {
            


            string customerId = Id;
            int customerIdInt = int.Parse(Id);

            var customerOrders = db.masterListItems
                .Where(o => o.CustomerId == customerIdInt)
                .ToList();

            if (!customerOrders.Any())
            {
                return View("NoData");
            }

            var model = new UserProfileInfoViewModel
            {
                CustomerId = customerId,
                FullName = $"{customerOrders.First().CustomerFName} {customerOrders.First().CustomerLName}",
                Country = customerOrders.First().CustomerCountry,
                City = customerOrders.First().CustomerCity,
                Street = customerOrders.First().CustomerStreet,
                Segment = customerOrders.First().CustomerSegment,

                TotalSpent = customerOrders.Sum(o => (double)(o.Sales ?? 0)),
                TotalItemsPurchased = customerOrders.Sum(o => o.OrderItemQuantity ?? 0),
                DeliveriesCompleted = customerOrders.Count(o => o.OrderStatus == "COMPLETE"),
                LateDeliveries = customerOrders.Count(o => o.LateDelivery == true),
                FirstPurchaseDate = customerOrders.Min(o => o.OrderDateDateOrders ?? DateTime.MinValue),

                TopProducts = customerOrders
                    .Where(p => p.ProductName != null)
                    .GroupBy(p => p.ProductName)
                    .Select(g => new TopProductViewModel
                    {
                        ProductName = g.Key,
                        Quantity = g.Sum(p => p.OrderItemQuantity ?? 0)
                    })
                    .OrderByDescending(g => g.Quantity)
                    .Take(3)
                    .ToList()
            };




            return View(model);
        }


        //This is the general information of each customer, this view is use to send users to informationsview
        [HttpGet]
        public ActionResult customerMain(int page = 1, int? Id = null, DateTime? filterFromDate = null, DateTime? filterToDate = null)
        {
            // Traer todos los datos (podrías ajustar este WHERE para otros filtros)
            IQueryable<masterListItems> query = db.masterListItems;

            // TEST: limitar a registros con Id < 200 (puedes quitar esto en producción)
            query = query.Where(r => r.Id < 200);

            // Agrupar por CustomerId y obtener solo un registro representativo por cliente
            var groupedQuery = query
                .GroupBy(c => c.CustomerId)
                .Select(g => g.FirstOrDefault());

            // Paginación
            int pageSize = 12;
            int totalItems = groupedQuery.Count();

            var data = groupedQuery
                .OrderBy(r => r.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
            //Region de viewbags, se mandan datos a la vista
            ViewBag.Page = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            
            




            return View(data);
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
