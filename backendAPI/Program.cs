using System.Text;
using backendAPI.Data;
using backendAPI.Interfaces;
using backendAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();


// dodavame DbContext kako servis i connection string za db
builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.AddCors(); // dodavame Cors kako servis
builder.Services.AddScoped<ITokenService,TokenService>();


//avtentikacija definirame servisot

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        var tokenKey = builder.Configuration["TokenKey"] ?? throw new Exception("Token key not found - program.cs");

        //how we validate the token 
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKey)),
            ValidateIssuer = false,
            ValidateAudience = false
        };
        
    });



var app = builder.Build();

// MIDDLEWARE

if (app.Environment.IsDevelopment()) // Configuring Middleware pipeline for development environment
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();


// Configure the HTTP request pipeline. MIDDLEWARE

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication(); // who are you ? 1st
app.UseAuthorization(); // are you allowed to do what you are trying to do ? 2nd

app.MapControllers();

app.Run();

// kako main funckija sluzi ova