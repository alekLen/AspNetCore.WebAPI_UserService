using AspNetCore.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

public class UserContext : DbContext
{
    public UserContext(DbContextOptions<UserContext> options) : base(options)
    {
        if (Database.EnsureCreated())
        {         
        }
    }
    public DbSet<User> Users { get; set; }
}
