using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using RideSharing.Models;

[assembly: OwinStartupAttribute(typeof(RideSharing.Startup))]
namespace RideSharing
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
           // CreateRolesAndUsers();
        }

    //    private void CreateRolesAndUsers()
    //    {
    //        ApplicationDbContext context = new ApplicationDbContext();

    //        var RoleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
    //        var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

    //        //In Startup iam creating first Admin Role and creating a default Admin User
    //        //test changes Grig
    //        if (!RoleManager.RoleExists("Admin"))
    //        {

    //            // first we create Admin rool
    //            var role = new IdentityRole
    //            {
    //                Name = "Admin"
    //            };
    //            RoleManager.Create(role);

    //            //Here we create a Admin super user who will maintain the website				

    //            var user = new ApplicationUser
    //            {
    //                UserName = "Grigore",
    //                Email = "bivolgri@yahoo.com"
    //            };

    //            string userPass = "ridesharing123";

    //            var checkUser = UserManager.Create(user, userPass);

    //            //Add default User to Role Admin
    //            if (checkUser.Succeeded)
    //            {
    //                var result1 = UserManager.AddToRole(user.Id, "Admin");
    //            }
    //        }

    //        // create Shop Manager role 
    //        if (!RoleManager.RoleExists("Passenger"))
    //        {
    //            var role = new IdentityRole
    //            {
    //                Name = "Passenger"
    //            };
    //            RoleManager.Create(role);
    //        }

    //        // create Driver role 
    //        if (!RoleManager.RoleExists("Driver"))
    //        {
    //            var role = new IdentityRole
    //            {
    //                Name = "Driver"
    //            };
    //            RoleManager.Create(role);
    //        }


    //    }
    }
}
