using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.WebUI.Models
{
    public class OrderEntry
    {
        public Product Product { get; set; }

        public Unit Unit { get; set; }

        public Supplier Supplier { get; set; }

        public Stock Stock { get; set; }

        public string ProductName { get; set; }

        public string UnitName { get; set; }

        public string SupplierName { get; set; }

        public int Quantity { get; set; }

        public decimal CostPerProduct { get; set; }

        public DateTime EntryDate { get; set; }

        public Guid StockId { get; set; }

        public string ProductNumber { get; set; }

        public Guid ProductId { get; set; }

        public Guid UnitId { get; set; }

        public Guid SupplierId { get; set; }

        public string SupplierAddress { get; set; }

    }

   
}