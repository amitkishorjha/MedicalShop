using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class OrderExit : BaseModel
    {
        [Required(ErrorMessage = "Please select product")]
        public Guid ProductId { get; set; }

        [Required(ErrorMessage = "Please select client")]
        public Guid ClientId { get; set; }

        [Required(ErrorMessage = "Please enter quantity")]
        public decimal Quatity { get; set; }

        [Required(ErrorMessage = "Please enter cost of product")]
        public decimal Cost { get; set; }

        [Required(ErrorMessage = "Please enter exit date")]
        [DisplayName("Exit date")]
        public DateTime ExitDate { get; set; }
    }
}
