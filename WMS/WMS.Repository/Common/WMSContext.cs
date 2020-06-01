using WMS.Models;
using System.Data.Entity;
using System;

namespace WMS.Repository.Common
{
    public class WMSContext : DbContext
    {
        public WMSContext() : base("WMSContext")    
        {
        }

        public DbSet<ActionMaster> ActionMaster { get; set; }

        public DbSet<UserMaster> UserMaster { get; set; }

        public DbSet<RoleMaster> RoleMaster { get; set; }

        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        public DbSet<RoleActionMapping> RoleActionMapping { get; set; }

        public DbSet<Client> Client { get; set; }

        public DbSet<Supplier> Supplier { get; set; }

        public DbSet<Unit> Unit { get; set; }

        public DbSet<Stock> Stock { get; set; }

        public DbSet<Product> Product { get; set; }

        public DbSet<OrderExit> OrderExit { get; set; }

        public DbSet<Category> Category { get; set; }

    }
  
}
