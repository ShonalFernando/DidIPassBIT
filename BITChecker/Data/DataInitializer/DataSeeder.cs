using BITChecker.Model;
using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Data.DataInitializer
{
    internal static class DataSeeder
    {
        public static async Task SeedAsync(AppDBContext context)
        {
            await context.Database.EnsureCreatedAsync();

            if (!context.Subjects.Any())
            {
                var subjects = ReadSubjectsFromCsv("Resources/BITSubjects.csv");
                context.Subjects.AddRange(subjects);
                await context.SaveChangesAsync();
            }
        }

        private static List<Subject> ReadSubjectsFromCsv(string path)
        {
            using var reader = new StreamReader(path);
            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);
            csv.Context.RegisterClassMap<SubjectMap>();
            return csv.GetRecords<Subject>().ToList();
        }

        public sealed class SubjectMap : ClassMap<Subject>
        {
            public SubjectMap()
            {
                Map(m => m.Code).Name("Code");
                Map(m => m.Name).Name("Name");
                Map(m => m.Semester).Name("Semester");
                Map(m => m.Credit).Name("Credits");
                Map(m => m.GPACredit).Name("GPA Credits");
                Map(m => m.CourseType).Convert(row => row.Row.GetField("Course Type")[0]);
            }
        }
    }
}
