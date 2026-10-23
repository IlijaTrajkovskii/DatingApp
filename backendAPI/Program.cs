using System.Text;
using backendAPI.Data;
using backendAPI.Helpers;
using backendAPI.Interfaces;
using backendAPI.Middleware;
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
builder.Services.AddScoped<IMemberRepository,MemberRepository>();
builder.Services.AddScoped<IPhotoService, PhotoService>();

// getSection CloudinarySettings doagja od appsettings.json
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

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



// Configure the HTTP request pipeline. MIDDLEWARE

app.UseMiddleware<ExceptionMiddleware>(); // expcetion middleware odi najgore vo pipeline-ot

if (app.Environment.IsDevelopment()) // Configuring Middleware pipeline for development environment
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();




app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod()
    .WithOrigins("http://localhost:4200","https://localhost:4200"));

app.UseAuthentication(); // who are you ? 1st
app.UseAuthorization(); // are you allowed to do what you are trying to do ? 2nd

app.MapControllers();

// pri stratup na app pravime automatski database i aplicirame pending migrations 
// i seednuvame databazata ako e prazna so seedUsers

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try
{
    var context = services.GetRequiredService<AppDbContext>();
    await context.Database.MigrateAsync();       // create the database or apply pending migrations
    await Seed.SeedUsers(context);              // seed the database
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occured during migrations!");
    
}

app.Run();

// kako main funckija sluzi ova