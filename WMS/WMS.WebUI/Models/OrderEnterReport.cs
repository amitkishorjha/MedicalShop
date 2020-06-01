using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebUI.Models
{
    public class OrderEnterReport
    {
        public string ProductName { get; set; }

        public string ProductNumber { get; set; }

        public string SupplierName { get; set; }

        public string UnitName { get; set; }

        public int Quantity { get; set; }

        public decimal CostPerProduct { get; set; }

        public string EntryDate { get; set; }


    }
}