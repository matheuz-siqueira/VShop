using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using VShop.Web.Services;
using VShop.Web.Services.Contracts;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


builder.Services.AddHttpClient("ProductApi", c => 
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:ProductApi"]);
});

builder.Services.AddHttpClient("CartApi", c => 
{
    c.BaseAddress = new Uri(builder.Configuration["ServiceUri:CartApi"]);
});



builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.AddAuthentication(options => 
{
    options.DefaultScheme = "Cookies";
    options.DefaultChallengeScheme = "oidc";
})
    .AddCookie("Cookies", c => 
    { 
        c.ExpireTimeSpan = TimeSpan.FromMinutes(10); 
        c.Events = new CookieAuthenticationEvents()
        {
            OnRedirectToAccessDenied = (context) => 
            {
                context.HttpContext.Response.Redirect(
                    builder.Configuration["ServiceUri:IdentityServer"] + "/Account/AccessDenied");
                return Task.CompletedTask;
            }
        };    
    })
    .AddOpenIdConnect("oidc", options =>
    {

        options.Events.OnRemoteFailure = context => 
        {
            context.Response.Redirect("/");
            context.HandleResponse(); 

            return Task.FromResult(0);
        };
        options.Authority = builder.Configuration["ServiceUri:IdentityServer"];
        options.GetClaimsFromUserInfoEndpoint = true; 
        options.RequireHttpsMetadata = false;
        options.ClientId = "vshop";
        options.ClientSecret = builder.Configuration["Client:Secret"];
        options.ResponseType = "code"; 
        options.ClaimActions.MapJsonKey("role", "role", "role");
        options.ClaimActions.MapJsonKey("sub", "sub", "sub");
        options.TokenValidationParameters.NameClaimType = "name"; 
        options.TokenValidationParameters.RoleClaimType = "role";
        options.Scope.Add("vshop");
        options.SaveTokens = true;
    }
);


var app = builder.Build();

app.UseCookiePolicy(new CookiePolicyOptions {MinimumSameSitePolicy = SameSiteMode.Lax });

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
