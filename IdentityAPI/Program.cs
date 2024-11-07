using IdentityAPI.Extensions;
using IdentityAPI.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.
    AddJsonFile("appsettings.json", true, true);
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<ApplicationDbContext>(opts =>
{
    opts.UseSqlServer(
        builder.Configuration["ConnectionStrings:Connection"]);
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("CorsPolicy", policy =>
    {
        policy
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("http://localhost:3000");
    });
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddIdentityServices(builder.Configuration);
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.UseCors("CorsPolicy");
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    await context.Database.MigrateAsync();
}
catch (Exception ex)
{
    //Log.Error("Error occured during migration", ex);
}

app.Run();
