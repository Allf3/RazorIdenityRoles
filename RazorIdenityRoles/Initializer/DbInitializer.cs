using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RazorIdenityRoles.Initializer
{
    public class DbInitializer
    {
        private static UserManager<IdentityUser> _userMananger;
        private static RoleManager<IdentityRole> _roleMananger;
        private static ILogger<DbInitializer> _logger;

        public static async Task Init(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, ILogger<DbInitializer> logger)
        {
            _userMananger = userManager;
            _roleMananger = roleManager;
            _logger = logger;

            if (await _userMananger.FindByNameAsync("IdenityAdmin") == null)
            {
                IdentityResult identityResult = await userManager.CreateAsync(new IdentityUser { UserName = "IdenityAdmin", Email = "Martinnybolvej@gmail.com" }, "P@ssw0rd");
                if (identityResult.Succeeded)
                {
                    IdentityResult identityResult1 = await _roleMananger.CreateAsync(new IdentityRole("Admin"));
                    if (identityResult1.Succeeded)
                    {
                        _logger.LogDebug("Init Role has been created");
                        var inituser = await userManager.FindByNameAsync("IdenityAdmin");
                        if (!await userManager.IsInRoleAsync(inituser, "Admin"))
                        {
                            await userManager.AddToRoleAsync(inituser, "Admin");
                            _logger.LogDebug("Init User has been added to role Admin");
                        }
                    }
                    else
                    {
                        throw new Exception("Init User has failed to be created");
                    }
                    _logger.LogDebug("Init User has been created");
                }
                else
                {
                    throw new Exception("Init User has failed to be created");
                }
            }
        }
    }
}
