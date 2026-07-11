namespace backendAPI.Data;

using backendAPI.Entities;
using Microsoft.EntityFrameworkCore;


// Pravime konstruktor za kreiranje na instanca od DbContext (t.e AppDbContext)
public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<AppUser> Users { get; set; }
}