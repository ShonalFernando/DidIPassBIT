using BITChecker.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Data
{
    class AppDbContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=appdata.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Subject>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.SubjectCode).IsRequired();
                entity.Property(e => e.SubjectName).IsRequired();
                entity.Property(e => e.Semester).IsRequired();
                entity.Property(e => e.Year).IsRequired();
                entity.Property(e => e.Credits).IsRequired();
                entity.Property(e => e.GPACredits).IsRequired();
            });
        }
    }
}
