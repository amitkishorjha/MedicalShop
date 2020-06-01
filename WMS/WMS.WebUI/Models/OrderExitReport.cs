using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebUI.Models
{
    public class OrderExitReport
    {
        public string ProductName { get; set; }

        public string ProductNumber { get; set; }

        public string ClientName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Cost { get; set; }

        public string ExitDate { get; set; }

    }
}