using BITChecker.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Data
{
    public class AppDBContext : DbContext
    {
        public DbSet<Subject> Subjects { get; set; }
        public DbSet<PerSubjectScore> PerSubjectScores { get; set; }
        public DbSet<GPA> GPAs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Get the path to the user's "AppData\Roaming" folder
            var appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

            // Build the path for "GPAChecker\Data" within "AppData"
            var databaseFolder = Path.Combine(appDataFolder, "GPAChecker", "Data");

            // Create the folder if it doesn't exist
            if (!Directory.Exists(databaseFolder))
            {
                Directory.CreateDirectory(databaseFolder);
            }

            // Define the path for the SQLite database
            var dbPath = Path.Combine(databaseFolder, "gpa_tracker.db");

            // Use the SQLite connection string
            optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // GPA 1:N PerSubjectScore
            modelBuilder.Entity<PerSubjectScore>()
                .HasOne(p => p.GPA)
                .WithMany(g => g.SubjectScores)
                .HasForeignKey(p => p.GPAId)
                .OnDelete(DeleteBehavior.Cascade);

            // Subject 1:N PerSubjectScore
            modelBuilder.Entity<PerSubjectScore>()
                .HasOne(p => p.Subject)
                .WithMany(s => s.PerSubjectScores)
                .HasForeignKey(p => p.SubjectId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
