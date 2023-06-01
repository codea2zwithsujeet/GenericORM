using Microsoft.EntityFrameworkCore;
using System;
using Web.API.Using.CommonORMLibrary.Models;

namespace Web.API.Using.CommonORMLibrary.Data
{
    public class EmployeeDbContext: DbContext
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder == null)
            {
                throw new ArgumentNullException(nameof(modelBuilder));
            }

            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Employee>().HasKey(e => e.EmployeeId);
        }

        public DbSet<Employee> Employee { get; set; }
    }
}
