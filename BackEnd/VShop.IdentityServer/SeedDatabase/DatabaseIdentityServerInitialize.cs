using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using VShop.IdentityServer.Configuration;
using VShop.IdentityServer.Data;

namespace VShop.IdentityServer.SeedDatabase;

public class DatabaseIdentityServerInitialize : IDatabaseSeedInitialize
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public DatabaseIdentityServerInitialize(UserManager<ApplicationUser> user, 
        RoleManager<IdentityRole> role)
    {
        _userManager = user; 
        _roleManager = role;
    }
    public void InitializeSeedRoles()
    {
        if(!_roleManager.RoleExistsAsync(IdentityConfiguration.Admin).Result)
        {
            IdentityRole roleAdmin = new()
            {
                Name = IdentityConfiguration.Admin,
                NormalizedName = IdentityConfiguration.Admin.ToUpper()
            };
            _roleManager.CreateAsync(roleAdmin).Wait(); 
        }

        if(!_roleManager.RoleExistsAsync(IdentityConfiguration.Client).Result)
        {
            IdentityRole roleClient = new()
            {
                Name = IdentityConfiguration.Client,
                NormalizedName = IdentityConfiguration.Client.ToUpper()
            };
            _roleManager.CreateAsync(roleClient).Wait(); 
        }
    }

    public void InitializeSeedUsers()
    {
        if(_userManager.FindByNameAsync("admin@example.com").Result == null)
        {
            ApplicationUser admin = new()
            {
                UserName = "admin",
                NormalizedUserName = "ADMIN", 
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM", 
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (1) 9876-1234",
                FirstName = "User",
                LastName = "Admin",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            IdentityResult resultAdmin = _userManager.CreateAsync(admin, "MyP4ssword@.").Result;
            if(resultAdmin.Succeeded)
            {
                _userManager.AddToRoleAsync(admin, IdentityConfiguration.Admin).Wait();

                var adminClaims = _userManager.AddClaimsAsync(admin, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{admin.FirstName} {admin.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Admin)
                }).Result;
            }
        }

        if(_userManager.FindByNameAsync("client@example.com").Result == null)
        {
            ApplicationUser client = new()
            {
                UserName = "client",
                NormalizedUserName = "CLIENT", 
                Email = "client@example.com",
                NormalizedEmail = "CLIENT@EXAMPLE.COM", 
                EmailConfirmed = true,
                LockoutEnabled = false,
                PhoneNumber = "+55 (1) 1234-5678",
                FirstName = "User",
                LastName = "Client",
                SecurityStamp = Guid.NewGuid().ToString()
            };
            IdentityResult resultClient = _userManager.CreateAsync(client, "MyP4ssword@.").Result;
            if(resultClient.Succeeded)
            {
                _userManager.AddToRoleAsync(client, IdentityConfiguration.Client).Wait();

                var clientClaims = _userManager.AddClaimsAsync(client, new Claim[]
                {
                    new Claim(JwtClaimTypes.Name, $"{client.FirstName} {client.LastName}"),
                    new Claim(JwtClaimTypes.GivenName, client.FirstName),
                    new Claim(JwtClaimTypes.FamilyName, client.LastName),
                    new Claim(JwtClaimTypes.Role, IdentityConfiguration.Client)
                }).Result;
            }
        }
    }
}
