using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class Category : BaseModel
    {
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
