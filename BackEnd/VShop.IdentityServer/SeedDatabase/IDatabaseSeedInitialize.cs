namespace VShop.IdentityServer.SeedDatabase;

public interface IDatabaseSeedInitialize
{
    void InitializeSeedRoles();
    void InitializeSeedUsers();
}
