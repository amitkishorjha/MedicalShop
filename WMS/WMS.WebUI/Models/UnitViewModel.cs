using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WMS.WebUI.Models
{
    public class UnitViewModel
    {
        public Guid UnitId { get; set; }

        public string UnitName { get; set; }

        public string Name { get; set; }

        public string IsHaveProduct { get; set; } = "False";

        public string Quanitiy { get; set; }

        public string ProductName { get; set; }

        public string ImageUrl { get; set; }

        public string SerialNo { get; set; }

        public string modelNo { get; set; }

        public decimal Price { get; set; }

        public string Category { get; set; }

        public string Description { get; set; }
    }
}