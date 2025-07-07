using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExcelToWeb.Models.ViewModels
{
    public class UserProfileInfoViewModel
    {
        public string CustomerId { get; set; }
        public string FullName { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public string Segment { get; set; }

        public double TotalSpent { get; set; }
        public int TotalItemsPurchased { get; set; }
        public int DeliveriesCompleted { get; set; }
        public int LateDeliveries { get; set; }
        public DateTime FirstPurchaseDate { get; set; }

        public List<TopProductViewModel> TopProducts { get; set; }
    }

    public class TopProductViewModel
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
    }


}