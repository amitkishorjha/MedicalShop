using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class RoleMaster :BaseModel
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter role name")]
        public string Name { get; set; }
    }
}
