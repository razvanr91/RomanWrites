using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RomanWrites.Data;
using RomanWrites.Enums;
using RomanWrites.Models;

namespace RomanWrites.Services
{
    public class DataService
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<BlogUser> _userManager;

        public DataService(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<BlogUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task ManageDataAsync()
        {
            // Create DB from the Migrations
            // the same as update-database
            await _context.Database.MigrateAsync();

            // Seed roles
            await SeedRolesAsync();
            
            // Seed Users
            await SeedUsersAsync();

        }

        private async Task SeedRolesAsync()
        {
            // Check if there are roles in the DB
            if (!_context.Roles.Any())
            {
                // Create a few roles
                foreach (var role in Enum.GetNames(typeof(BlogRole)))
                {
                    await _roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            return;
        }

        private async Task SeedUsersAsync()
        {
            if(_context.Users.Any())
            {
                return;
            }

            var adminUser = new BlogUser()
            {
                FirstName = "Razvan",
                LastName = "Roman",
                Email = "razvan.roman91@outlook.com",
                EmailConfirmed = true,
                UserName = "razvan.roman91@outlook.com"
            };

            await _userManager.CreateAsync(adminUser, "Abc&1!");

            await _userManager.AddToRoleAsync(adminUser,BlogRole.Administrator.ToString());

            await _context.SaveChangesAsync();
        }

    }
}