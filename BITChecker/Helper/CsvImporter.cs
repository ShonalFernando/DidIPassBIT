using BITChecker.Data;
using BITChecker.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Helper
{
    public static class CsvImporter
    {
        public static void ImportSubjectsFromCsv(string csvFilePath)
        {
            if (!File.Exists(csvFilePath))
                throw new FileNotFoundException("CSV file not found.", csvFilePath);

            var lines = File.ReadAllLines(csvFilePath);

            using (var db = new AppDbContext())
            {
                foreach (var line in lines.Skip(1)) // Skip header
                {
                    var parts = line.Split(',');

                    if (parts.Length < 6)
                        continue; // skip invalid lines

                    var subjectCode = parts[0].Trim();

                    // Check if subject with this code already exists
                    bool exists = db.Subjects.Any(s => s.SubjectCode == subjectCode);

                    if (exists)
                        continue; // skip adding this subject

                    var subject = new Subject
                    {
                        SubjectCode = subjectCode,
                        SubjectName = parts[1].Trim(),
                        Semester = int.Parse(parts[2].Trim()),
                        Year = int.Parse(parts[3].Trim()),
                        Credits = int.Parse(parts[4].Trim()),
                        GPACredits = int.Parse(parts[5].Trim())
                    };

                    db.Subjects.Add(subject);
                }

                db.SaveChanges(); // Save after all new subjects are added
            }
        }
    }
}
