using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace WMS.Models
{
    public class Product : BaseModel
    {
        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

        [Display(Name="Serial No.")]
        [Required(ErrorMessage = "Please enter serial number ")]
        public string Number { get; set; }

        [Display(Name = "Model No.")]
        [Required(ErrorMessage = "Please enter model number ")]
        public string modelNo { get; set; }

        [Required(ErrorMessage = "Please enter price ")]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Please enter Category ")]
        [DisplayName("Category")]
        public Guid CategoryId { get; set; }

        public string Description { get; set; }

        [NotMapped]
        [DisplayName("Category")]
        public string CategoryName { get; set; }

        [Required(ErrorMessage ="please select image")]
        public string ImagePath { get; set; }

        [NotMapped]
        [Display(Name="Image 1")]
        public HttpPostedFileBase ImageUrl { get; set; }


        public string ImagePath1 { get; set; }

        [NotMapped]
        [Display(Name = "Image 2")]
        public HttpPostedFileBase ImageUrl1 { get; set; }
    }
}
