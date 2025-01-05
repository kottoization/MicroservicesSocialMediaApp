using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1) Dostêp do HttpContext w kontrolerach i przy tworzeniu HttpClientów
builder.Services.AddHttpContextAccessor();

// 2) Dodajemy kontrolery i widoki
builder.Services.AddControllersWithViews();

// 3) Tworzymy named HttpClient z dynamicznym ustawianiem nag³ówka Authorization
builder.Services.AddHttpClient("PostApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:PostApi"]);
}).ConfigureHttpClient((serviceProvider, httpClient) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var token = httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type == "JWT")?.Value;

    if (!string.IsNullOrEmpty(token))
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
});

builder.Services.AddHttpClient("CommentApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:CommentApi"]);
}).ConfigureHttpClient((serviceProvider, httpClient) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var token = httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type == "JWT")?.Value;

    if (!string.IsNullOrEmpty(token))
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
});

builder.Services.AddHttpClient("IdentityApi", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ApiUrls:IdentityApi"]);
}).ConfigureHttpClient((serviceProvider, httpClient) =>
{
    var httpContextAccessor = serviceProvider.GetRequiredService<IHttpContextAccessor>();
    var token = httpContextAccessor.HttpContext?.User?.Claims
        .FirstOrDefault(c => c.Type == "JWT")?.Value;

    if (!string.IsNullOrEmpty(token))
    {
        httpClient.DefaultRequestHeaders.Authorization =
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
    }
});

// 4) Konfiguracja Cookie auth
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

var app = builder.Build();

// Middlewares
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
