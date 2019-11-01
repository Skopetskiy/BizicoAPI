using Database;
using Database.Models;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Controllers
{
    public class Seeder
    {
        private readonly UserManager<User> _usermanager;
        private readonly RoleManager<Role> _rolemanager;
        private readonly FreelanceContext _context;

        public Seeder(UserManager<User> usermanager, FreelanceContext context, RoleManager<Role> roleManager)
        {
            _usermanager = usermanager;
            _context = context;
            _rolemanager = roleManager;
        }
        public async Task Seed()
        {
            string[] roleNames = { "Admin", "Moderator", "Client" };

            foreach (var x in roleNames)
            {
                if (_context.Roles.Where(r => r.Name == x).Count() == 0)
                {
                    await _rolemanager.CreateAsync(new Role { Name = x });
                }
            }
            _context.SaveChanges();

            var res = _context.Roles.First(x => x.Name == "Admin");
            User user = new User { Email = "admin@gmail.com", UserName = "admin" };
            await _usermanager.CreateAsync(user, "admin123!");
 
                await _usermanager.AddToRoleAsync(user, "admin");
        }
    }
}
