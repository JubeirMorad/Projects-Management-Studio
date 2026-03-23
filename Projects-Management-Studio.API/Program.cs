using Projects_Management_Studio.App.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Infra.Authentication;
using Projects_Management_Studio.Infra.Data;
using Projects_Management_Studio.Infra.Repostories;
using Projects_Management_Studio.App.Services.Interfaces;
using Projects_Management_Studio.App.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// add DbContext
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string was not found.");
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(connectionString);
});

// add userRepository
builder.Services.AddScoped<IUserRepository, UserRepository>();

// add passwordHasher
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// add AuthenticationService
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
