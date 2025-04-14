using ImagineHubAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ImagineHubAPI.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
}