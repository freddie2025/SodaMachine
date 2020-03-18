using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace SodaMachineRazorUI.Pages
{
    public class GenerateAdminModel : PageModel
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;
        private const string adminRole = "Admin";

        public bool RoleCreated { get; set; }
        public string AdminUserId { get; set; }

        public GenerateAdminModel(RoleManager<IdentityRole> roleManager,
                                  UserManager<IdentityUser> userManager,
                                  IConfiguration config)
        {
            _roleManager = roleManager; 
            _userManager = userManager;
            _config = config;
        }

        public async Task OnGet()
        {
            var roleExist = await _roleManager.RoleExistsAsync(adminRole);

            if (roleExist == false)
            {
                await _roleManager.CreateAsync(new IdentityRole(adminRole));
                RoleCreated = true;
            }

            string userEmail = _config.GetValue<string>("AdminUser");
            var user = await _userManager.FindByEmailAsync(userEmail);

            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, adminRole);
                AdminUserId = user.Id;

            }
        }
    }
}