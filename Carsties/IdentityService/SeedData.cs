using System.Security.Claims;
using IdentityModel;
using IdentityService.Data;
using IdentityService.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace IdentityService;

public class SeedData
{
    public static void EnsureSeedData(WebApplication app)
    {
        using IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        var context = scope.ServiceProvider.GetService<ApplicationDbContext>();
        context.Database.Migrate();

        UserManager<ApplicationUser> userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        if (userMgr.Users.Any()) return;

        AddUser(userMgr, "alice", "AliceSmith@email.com", "Pass123$", "Alice Smith");
        AddUser(userMgr, "bob", "BobSmith@email.com", "Pass123$", "Bob Smith");
    }

    private static void AddUser(UserManager<ApplicationUser> userMgr, string name, string mail, string password, string fullName)
    {
        ApplicationUser user = userMgr.FindByNameAsync("bob").Result;
        if (user == null)
        {
            user = new ApplicationUser
            {
                UserName = name,
                Email = mail,
                EmailConfirmed = true
            };
            IdentityResult result = userMgr.CreateAsync(user, password).Result;

            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

            Claim[] claims = new Claim[]{ new Claim(JwtClaimTypes.Name, fullName) };
            result = userMgr.AddClaimsAsync(user, claims).Result;
            if (!result.Succeeded) throw new Exception(result.Errors.First().Description);

            Log.Debug($"#{name} created");
        }
        else
        {
            Log.Debug($"{name} already exists");
        }
    }
}
