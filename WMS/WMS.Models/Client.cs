using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class Client : BaseModel
    {
        [Required(ErrorMessage ="Please enter name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter Surname ")]
        public string Surname { get; set; }

        public string Company { get; set; }

        [Required(ErrorMessage = "Please enter email ")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Please enter client phone no ")]
        [DisplayName("Phone")]
        public string Phone1 { get; set; }

        [DisplayName("Secondry phone")]
        public string Phone2 { get; set; }

        [Required(ErrorMessage = "Please enter client address ")]
        public string Address { get; set; }

        public string Description { get; set; }
    }
}
