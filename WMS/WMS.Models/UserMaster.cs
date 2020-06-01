using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
    public class UserMaster :BaseModel
    {
        [DisplayName("Name")]
        [Required(ErrorMessage = "Please enter user name")]
        public string Username { get; set; }

        [DisplayName("First Name")]
        [Required(ErrorMessage = "Please enter first name")]
        public string FirstName { get; set; }

        [DisplayName("Middle Name")]
        [Required(ErrorMessage = "Please enter middle name")]
        public string MiddleName { get; set; }

        [DisplayName("Last Name")]
        [Required(ErrorMessage = "Please enter last name")]
        public string LastName { get; set; }

        [DisplayName("Last login date")]
        public string LastLoginDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Password { get; set; }

    }
}
