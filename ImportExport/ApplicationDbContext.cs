
using ImportExport.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ImportExport
{
   
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<City> Cities { get; set; }


    }
}
