using Microsoft.EntityFrameworkCore;
using SpotiAPI.Models;

namespace SpotiAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public DbSet<Song> Songs { get; set; }
         
    }
}
