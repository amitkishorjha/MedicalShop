using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WMS.Models
{
  public class RoleActionMapping:BaseModel
    {
        public Guid RoleMasterId { get; set; }

        public Guid ActionMasterId { get; set; }
    }
}
