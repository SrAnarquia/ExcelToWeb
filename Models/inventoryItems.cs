//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExcelToWeb.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class inventoryItems
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string ItemDescription { get; set; }
        public decimal ItemPrice { get; set; }
        public string ItemImage { get; set; }
        public string CategoryName { get; set; }
        public string DepartmentName { get; set; }
        public string ProductImage { get; set; }
        public int Stock { get; set; }
        public bool IsActive { get; set; }
        public Nullable<System.DateTime> CreatedAt { get; set; }
    }
}
