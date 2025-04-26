using BITChecker.Data;
using BITChecker.Model;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {

        public CalculatorViewModel()
        {
            // Initialize Lists
            _sortedSubjects = new ObservableCollection<Subject>();
            _semestersList = new ObservableCollection<int>();

            PopulateData();
        }

        // Populate
        private void PopulateData()
        {
            using (var db = new AppDbContext())
            {
                SortedSubjects = new ObservableCollection<Subject>(db.Subjects.ToList());
            }

            _yearList = new ObservableCollection<int> { 1, 2, 3 };
            _gradeOptions = new ObservableCollection<string>
            {
                "A+",
                "A",
                "B+",
                "B",
                "C+",
                "C",
                "D+",
                "D",
                "E",
                "Pass",
                "Fail"
            };
    }

        private void FilterSort()
        {
            using (var db = new AppDbContext())
            {
                SortedSubjects = new ObservableCollection<Subject>(db.Subjects.Where(s => s.Semester == FilterSemester));
            }
        }

        private void AdjustSemesters()
        {
            SemestersList = new ObservableCollection<int>
            {
                (FilterYear * 2) - 1,
                FilterYear * 2
            };

            FilterSemester = (FilterYear * 2) - 1;
            // MessageBox.Show(FilterYear.ToString());
        }

        private void PostSubjectSelect()
        {
            if (SelectedSubject is not null)
            {
                if (SelectedSubject.GPACredits is 0)
                {
                    GradeOptions.Clear();
                    GradeOptions = new ObservableCollection<string>
                {
                    "Pass",
                    "Fail"
                };
                }
                else
                {
                    GradeOptions = new ObservableCollection<string>
                {
                    "A+",
                    "A",
                    "B+",
                    "B",
                    "C+",
                    "C",
                    "D+",
                    "D",
                    "E"
                };
                } 
            }
        }
    }
}
