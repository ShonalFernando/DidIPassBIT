using BITChecker.Data;
using BITChecker.Helper;
using BITChecker.View;
using System.Configuration;
using System.Data;
using System.Windows;

namespace BITChecker;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // 1. Create DB if needed
        using (var db = new AppDbContext())
        {
            db.Database.EnsureCreated();
        }

        // 2. Load Subjects for the first time
        CsvImporter.ImportSubjectsFromCsv("Data/Subjects.csv");


        // 3. Launch Main Window
        new ShellWindow().Show();  // <-- create instance & Show it to the user

    }
}

