using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class ActionMaster:BaseModel
    {
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [DisplayName("Full action name")]
        [Required(ErrorMessage = "Please enter full action name")]
        public string FullActionName { get; set; }

        [DisplayName("Controller Name")]
        [Required(ErrorMessage = "Please enter controller name")]
        public string ControllerName { get; set; }

    }
}
