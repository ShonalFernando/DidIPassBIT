using BITChecker.Command;
using BITChecker.Data;
using BITChecker.Helper;
using BITChecker.Model;
using BITChecker.View;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        private readonly GradeToDecimalConverter _gradeConverter = new GradeToDecimalConverter();
        private readonly Action<object> _navigate;

        public CalculatorViewModel(Action<object> navigate)
        {
            // Initialize Lists
            _sortedSubjects = new ObservableCollection<Subject>();
            _semestersList = new ObservableCollection<int>();
            _subjectScores = new ObservableCollection<SubjectScore>();

            // Initialize Actions
            _navigate = navigate;

            // Initialize Commands
            AddSubjectCommand = new RelayCommand(_ => AddSubject(), CanAddSubject);
            GoToResultsCommand = new RelayCommand(_ => NavigateToResults());

            // Anor mality Normalizing
            ExamYear = DateTime.Now.Year;

            PopulateData();
        }

        // Navigation
        private void NavigateToResults()
        {
            // Invoke aka Fire the Action
            _navigate.Invoke(new ResultView(SubjectScores));
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

        private void AddSubject()
        {
            var _addedSubject = new SubjectScore()
            {
                Semester = SelectedSubject.Semester,
                Year = SelectedSubject.Year,
                SubjectCode = SelectedSubject.SubjectCode,
                SubjectID = SelectedSubject.Id,
                SubjectName = SelectedSubject.SubjectName,
                Grade = (decimal)_gradeConverter.Convert(SelectedScore, typeof(decimal), null, CultureInfo.InvariantCulture),
                Credit = SelectedSubject.GPACredits,
                isEnhancement = SelectedSubject.GPACredits is 0,
                Passed = false
            };

            

            if(_addedSubject.isEnhancement is true)
            {
                if(SelectedScore is "Pass")
                {
                    _addedSubject.Passed = true;
                }
            }
            else
            {
                if(_addedSubject.Grade >= 1)
                {
                    _addedSubject.Passed = true;
                }
            }

            // Check if a user adds repeat without original sitting
            if(CheckOriginalSitting() is not false && DuplicateCheck() is not false)
            {
                _addedSubject.isRepeat = IsRepeat;
                _addedSubject = SetRepeatable(_addedSubject);

                // If not an ehnancement and repeat, then cap the score
                if(_addedSubject.isEnhancement is not true)
                {
                    if(_addedSubject.Grade > 2.0m)
                    {
                        _addedSubject.Grade = 2.0m;
                    }
                }

                _addedSubject.Weight = _addedSubject.Credit * _addedSubject.Grade;
                YearNormalizeWarning();
                SubjectScores.Add(_addedSubject);
            }

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
