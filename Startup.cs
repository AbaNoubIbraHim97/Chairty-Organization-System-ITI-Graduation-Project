using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using WebApplication1.Models;
using Owin;
[assembly: OwinStartupAttribute(typeof(WebApplication1.Startup))]
namespace WebApplication1
{
    public partial class Startup
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateDefaultRolesAndUsers();
        }
        public void CreateDefaultRolesAndUsers()
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            IdentityRole role = new IdentityRole();
            if (!roleManager.RoleExists("Administrator"))
            {
                role.Name = "Administrator";
                roleManager.Create(role);
                ApplicationUser user = new ApplicationUser();
                user.UserName = "Administrator";
                user.Email = "Administrator@gmail.com";
                user.UserType = "Administrator";
                var check = userManager.Create(user, "Mm123-");
                if (check.Succeeded)
                {
                    userManager.AddToRole(user.Id, "Administrator");
                }
            }
        }
    }
}
