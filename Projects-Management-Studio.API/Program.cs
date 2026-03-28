using Projects_Management_Studio.App.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Projects_Management_Studio.App.Interfaces.Repositories;
using Projects_Management_Studio.Infra.Authentication;
using Projects_Management_Studio.Infra.Data;
using Projects_Management_Studio.Infra.Repostories;
using Projects_Management_Studio.App.Services.Interfaces;
using Projects_Management_Studio.App.Services;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Projects_Management_Studio.API.ApiServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("0000_SUPER_SECRET_KEY_123_11_23354111"))
        };
    });

builder.Services.AddAuthorization();


// add DbContext
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string was not found.");
builder.Services.AddDbContext<AppDbContext>(o =>
{
    o.UseSqlServer(connectionString);
});

// add Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();

// add passwordHasher
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// add Services
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProjectService, ProjectService>();

builder.Services.AddScoped<IJwtProvider, JwtProvider>();


// add Api Services >> these are services that will be used by the controllers to interact with the application layer
builder.Services.AddScoped<ICurrrentUserService, CurrentUserService>();
builder.Services.AddHttpContextAccessor();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// test auth in this point 
app.MapGet("/", () => "you are authenticated.").RequireAuthorization().WithDisplayName("Test");

app.Run();
