using System.Security.Principal;
using Duende.IdentityServer;
using Duende.IdentityServer.Models;

namespace VShop.IdentityServer.Configuration;

public class IdentityConfiguration
{
    public const string Admin = "Admin";
    public const string Client = "Client";

    public static IEnumerable<IdentityResource> IdentityResources => 
        new List<IdentityResource>
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile()
        };

    public static IEnumerable<ApiScope> ApiScopes => 
        new List<ApiScope>
        {
            new ApiScope("vshop", "Vshop Server"),
            new ApiScope(name: "read", "Read data."),
            new ApiScope(name: "write", "Write data."),
            new ApiScope(name: "delete", "Delete data."),
        };

    public static IEnumerable<Client> Clients =>
        new List<Client>
        {   
            new Client
            {
                ClientId = "client",
                ClientSecrets = { new Secret("88d73035-4cbb-43b0-a5c7-a06c0661bd74".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials, 
                AllowedScopes = {"read", "write", "profile"}
            }, 
            new Client
            {
                ClientId = "vshop",
                ClientSecrets = { new Secret ("88d73035-4cbb-43b0-a5c7-a06c0661bd74".Sha256())}, 
                AllowedGrantTypes = GrantTypes.Code, 
                RedirectUris = {"http://localhost:5054/signin-oidc"}, 
                PostLogoutRedirectUris = {"http://localhost:5054/signout-callback-oidc"},
                AllowedScopes = new List<string>()
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        IdentityServerConstants.StandardScopes.Email,
                        "vshop"
                    }
            }
        };
}
