using ExcelToWeb.Models;
using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Web.UI.WebControls;

namespace ExcelToWeb.Controllers
{

    
    public class HomeController : Controller
    {
        //Creamo contexto para manejo de consultas LINQ
        private Supply_ChainEntities1 db = new Supply_ChainEntities1();


        [HttpGet]
        public ActionResult Index()
        {
            //Listado de top 10 paises - Se limita a 2016 en adelante los datos
           var countryTop10= db.masterListItems
                .Where(i=> i.OrderDateDateOrders.Value.Year >2016)
                .GroupBy(o => o.OrderCountry) //Seguido creamos diccioanrio con select para dentro del group by organizar info
                .Select( g => new 
                {
                    Country=g.Key,
                    Sales=g.Sum(x => x.Sales)
                })//Ahora con los datos del select hacemos el orden
                .OrderByDescending(x =>x.Sales) //Ahora le decimos que de lo agrupado tome 10 porque ya estan en orden
                .Take(10).ToList();
            //Luego se usa esta agrupacion para retonarna a la vista
            ViewBag.CountrySalesDataJson = JsonConvert.SerializeObject(countryTop10);

            //Listado de top 10 categorias - Se limita a 2016 en adelante los datos
            var categoryTop10 = db.masterListItems
                .Where(i=> i.OrderDateDateOrders.Value.Year > 2016)
                .GroupBy(i => i.CategoryName)
                .Select(g => new
                {
                    Category= g.Key,
                    Sales=g.Sum(x=> x.Sales)

                })
                .OrderByDescending(x => x.Sales)
                .Take(10).ToList();

            //Luego se usa esta agrupacion para retornar a la vista
            ViewBag.CategorySalesDataJson = JsonConvert.SerializeObject(categoryTop10);




            // Agrupar ganancias por mes y año  - Se limita a 2016 en adelante los datos
            var monthlyProfit = db.masterListItems
                .Where(i => i.OrderDateDateOrders != null && i.OrderDateDateOrders.Value.Year > 2016) //Se filtra buscando null
                .GroupBy(i => new
                {
                    Year = i.OrderDateDateOrders.Value.Year,
                    Month = i.OrderDateDateOrders.Value.Month
                }
                )
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Profit = g.Sum(x => x.OrderProfitPerOrder)
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            // Convertimos el nombre del mes para que sea legible en la gráfica - Se limita a 2016 en adelante los datos
            var monthlyProfitFormatted = monthlyProfit.Select(m => new
            {
                MonthYear = new DateTime(m.Year, m.Month, 1).ToString("MMM yyyy"),
                Profit = m.Profit
            }).ToList();

            // Enviamos el JSON a la vista
            ViewBag.MonthlyProfitDataJson = JsonConvert.SerializeObject(monthlyProfitFormatted);

            //Agrupamos por delay por mes y año - Se limita a 2016 en adelante los datos
            var monthlyDelay = db.masterListItems
                .Where(i => i.OrderDateDateOrders != null && i.LateDelivery == true && i.OrderDateDateOrders.Value.Year > 2016 )
                .GroupBy(i => new
                {
                    Year = i.OrderDateDateOrders.Value.Year,
                    Month = i.OrderDateDateOrders.Value.Month
                })
                .Select(g => new
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    Delay = g.Count() // Solo contamos cuántos hubo con retraso
                })
                .OrderBy(x => x.Year)
                .ThenBy(x => x.Month)
                .ToList();

            // Formateamos para la gráfica
            var monthlyDelayFormatted = monthlyDelay.Select(m => new
            {
                MonthYear = new DateTime(m.Year, m.Month, 1).ToString("MMM yyyy"),
                Delay = m.Delay
            }).ToList();

            // Enviar a la vista
            ViewBag.MonthlyDelayDataJson = JsonConvert.SerializeObject(monthlyDelayFormatted);

            return View();
        }


        public ActionResult Items() 
        {

            return View();
        }


        /*This method closes the DB connections and cleans up memory*/
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