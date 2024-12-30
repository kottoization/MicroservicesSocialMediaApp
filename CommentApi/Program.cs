using CommentApi;
using CommentApi.IdentityAPI;
using CommentAPI.Services;
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

// Register services
builder.Services.AddSingleton<ICommentService, CommentService>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
