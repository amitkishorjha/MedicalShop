using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WMS.Models;

namespace WMS.WebUI.Models
{
    public class OrderExit
    {
        public Product Product { get; set; }

        public Guid OrderExitId { get; set; }

        public Client Client { get; set; }

        public OrderExit orderExit { get; set; }

        public string ProductName { get; set; }

        public string ClientName { get; set; }

        public decimal Quantity { get; set; }

        public decimal Cost { get; set; }

        public DateTime ExitDate { get; set; }

        public string ProductNumber { get; set; }

        public Guid ProductId { get; set; }

        public Guid ClientId { get; set; }

        public string ClientAddress { get; set; }

    }
}