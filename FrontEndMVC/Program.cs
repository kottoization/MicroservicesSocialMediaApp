using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add HttpClient for API communication
builder.Services.AddHttpClient("PostApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:PostApi"]);
});
builder.Services.AddHttpClient("CommentApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:CommentApi"]);
});
builder.Services.AddHttpClient("IdentityApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:IdentityApi"]);
});

// Add authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Configure middleware
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();