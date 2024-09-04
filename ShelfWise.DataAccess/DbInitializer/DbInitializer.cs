using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShelfWise.DataAccess.Data;
using ShelfWise.Models;
using ShelfWise.Utils;

namespace ShelfWise.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;
        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _db = db;
        }

        public void Initialize()
        {
            // Push migration in case not applied yet
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception ex)
            { }

            // Create roles if not exist
            if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();

                // Create admin role if roles not created
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@kitepea.edu.vn",
                    Email = "admin@kitepea.edu.vn",
                    Name = "Dexter Admin",
                    PhoneNumber = "012791281921",
                    State = "HCM",
                    City = "Thu Duc",
                    PostalCode = "700000",
                    StreetAddress = "221B Baker Street",
                }, "Admin@123").GetAwaiter().GetResult();

                ApplicationUser admin = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@kitepea.edu.vn");
                _userManager.AddToRoleAsync(admin, StaticDetails.Role_Admin).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
