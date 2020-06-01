namespace WMS.Repository.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using WMS.Models;

    public class Configuration : DbMigrationsConfiguration<Common.WMSContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(Common.WMSContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
            if (context.UserMaster.Where(x => x.Username == "Admin123" && x.IsActive == true).Count() == 0)
            {
                UserMaster user = new UserMaster();
                user.UniqueId = Guid.NewGuid();
                user.Username = "Admin123";
                user.FirstName = "Admin";
                user.MiddleName = "12";
                user.LastName = "3";
                user.Password = "Admin123";
                user.IsActive = true;
                user.CreatedDate = DateTime.Now;
                user.CreatedBy = "System";

                context.UserMaster.Add(user);

                RoleMaster role = new RoleMaster
                {
                    UniqueId = Guid.NewGuid(),
                    Name = "Admin",
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System"
                };

                context.RoleMaster.Add(role);

                UserRoleMapping userRoleMapping = new UserRoleMapping
                {
                    UniqueId = Guid.NewGuid(),
                    UserMasterId = user.UniqueId,
                    RoleMasterId = role.UniqueId,
                    IsActive = true,
                    CreatedDate = DateTime.Now,
                    CreatedBy = "System"
                };

                context.UserRoleMapping.Add(userRoleMapping);

                context.SaveChanges();
                base.Seed(context);
            }
        }
    }
}
