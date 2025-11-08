namespace StaffManagement.Utils;

using StaffManagement.Models;
using Microsoft.EntityFrameworkCore;

public class StaffDbContext : DbContext
{
    public StaffDbContext(DbContextOptions<StaffDbContext> options) : base(options) { }
    
    public DbSet<Staff> Staff { get; set; } 
}