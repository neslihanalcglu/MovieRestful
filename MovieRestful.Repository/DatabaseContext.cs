using Microsoft.EntityFrameworkCore;
using MovieRestful.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MovieRestful.Repository
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {

        }

        public DbSet<Movie> mytable { get; set; }
        public DbSet<Trending> Trendings { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //   => optionsBuilder.UseNpgsql("Host=localhost;Database=RestfulAPI;Username=postgres;Password=3736");
    }
}
