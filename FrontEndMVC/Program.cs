using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Add IHttpContextAccessor (required for JwtAuthorizationHandler)
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

// Add JwtAuthorizationHandler
builder.Services.AddTransient<JwtAuthorizationHandler>();

// Add HttpClient for API communication
builder.Services.AddHttpClient("PostApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:PostApi"]);
}).AddHttpMessageHandler<JwtAuthorizationHandler>(); ;
builder.Services.AddHttpClient("CommentApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:CommentApi"]);
}).AddHttpMessageHandler<JwtAuthorizationHandler>(); ;
builder.Services.AddHttpClient("IdentityApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:IdentityApi"]);
}).AddHttpMessageHandler<JwtAuthorizationHandler>(); ;

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