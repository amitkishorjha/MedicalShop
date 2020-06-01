using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class Stock : BaseModel
    {
        [Required(ErrorMessage = "Please select product")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage ="Please select supplier")]
        public Guid SupplierId { get; set; }

        [Required(ErrorMessage = "Please enter quantity")]
        public int Quatity { get; set; }

        [Required(ErrorMessage = "Please select unit")]
        public Guid UnitId { get; set; }

        [Required(ErrorMessage = "Please enter cost of product")]
        [DisplayName("Cost / product")]
        public decimal CostPerProduct { get; set; }

        [Required(ErrorMessage = "Please enter enrty date")]
        [DisplayName("Entry date")]
        public DateTime EntryDate { get; set; }

    }
}
