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
using System.Windows.Threading;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        private readonly GradeToDecimalConverter _gradeConverter = new GradeToDecimalConverter();
        private readonly Action<object> _navigate;
        private ScoreEditor? _scoreEditor;
        private ScoreDeleter? _scoreDeleter;

        public CalculatorViewModel(Action<object> navigate)
        {
            // Initialize Lists
            _sortedSubjects = new ObservableCollection<Subject>();
            _semestersList = new ObservableCollection<int>();
            _subjectScores = new ObservableCollection<SubjectScore>();
           
            // Initialize Dialogs
            _scoreEditor = new ScoreEditor();
            _scoreEditor.DataContext = this;
            _scoreDeleter = new ScoreDeleter();
            _scoreDeleter.DataContext = this;

            // Initialize Actions
            _navigate = navigate;

            // Initialize Commands
            AddSubjectCommand = new RelayCommand(_ => AddSubject(), CanAddSubject);
            GoToResultsCommand = new RelayCommand(_ => NavigateToResults(), CanNavigateToResult);
            EditSubjectCommand = new RelayCommand(_ => EditSubject(), CanEditorDeleteSubject);
            DeleteSubjectCommand = new RelayCommand(_ => DeleteSubject(), CanEditorDeleteSubject);
            UpdateSubjectCommand = new RelayCommand(_ => UpdateSubject(), CanEditorDeleteSubject);
            CancelDeleteCommand = new RelayCommand(_ => CancelDelete());
            // Anor mality Normalizing
            ExamYear = DateTime.Now.Year;

            SelectedScore = "A+";

            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromSeconds(1);
            _timer.Tick += _timer_Tick;

            PopulateData();
        }

        private void _timer_Tick(object? sender, EventArgs e)
        {
            CountdownValue--;

            ProgressValue = (CountdownValue / (double)_totalSeconds) * 100;

            if (CountdownValue <= 0)
            {
                DeleteInvoke();
                _scoreDeleter.Close();
                _timer.Stop();
            }
        }

        // Navigation
        private void NavigateToResults()
        {
            if(CanNavigate())
                // Continue with complete semesters
                _navigate.Invoke(new ResultView(SubjectScores));

        }

        // Edit Subject
        private void EditSubject()
        {
            // Initialize Dialogs
            _scoreEditor = new ScoreEditor();
            _scoreEditor.DataContext = this;
            _scoreEditor.ShowDialog();
        }

        // Delete Subject
        private void DeleteSubject()
        {
            // OTHER INITIALIZATION : TIMER
            CountdownValue = _totalSeconds;
            ProgressValue = 100;
            _scoreDeleter = new ScoreDeleter();
            _scoreDeleter.DataContext = this;
            _scoreDeleter.Show();
            _timer.Start();
        }

        private void DeleteInvoke()
        {
            SubjectScores.Remove(SelectedSubjectScore);
        }

        private void CancelDelete()
        {
            _timer.Stop();

            if (_scoreDeleter is not null)
                _scoreDeleter.Close();
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
                    "A-",
                    "B+",
                    "B",
                    "B-",
                    "C+",
                    "C",
                    "C-",
                    "D+",
                    "D",
                    "E",
                "Pass",
                "Fail"
            };
        }

        private void UpdateSubject()
        {
            // Store Selected Item Index
            var updateIndex = SubjectScores.IndexOf(SelectedSubjectScore);

            // Now make changes
            var updatableSubject = SelectedSubjectScore;
            updatableSubject.Grade = (decimal)_gradeConverter.Convert(UpdatedScore, typeof(decimal), null, CultureInfo.InvariantCulture);


            if (updatableSubject.isEnhancement is true)
            {
                if (SelectedScore is "Pass")
                {
                    updatableSubject.Passed = true;
                }
            }
            else
            {
                if (updatableSubject.Grade >= 1)
                {
                    updatableSubject.Passed = true;
                }
            }

            // Check if a user adds repeat without original sitting
            updatableSubject = SetRepeatable(updatableSubject);

                // If not an ehnancement and repeat, then cap the score
                if (updatableSubject.isEnhancement is not true && updatableSubject.isRepeat)
                {
                    if (updatableSubject.Grade > 2.0m)
                    {
                        updatableSubject.Grade = 2.0m;
                    }
                }

                updatableSubject.Weight = updatableSubject.Credit * updatableSubject.Grade;
                YearNormalizeWarning();

            SubjectScores.Remove(SubjectScores[updateIndex]);
            SubjectScores.Add(updatableSubject);

            _scoreEditor.Close();
            _scoreEditor = null;
            GC.Collect();
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



            if (_addedSubject.isEnhancement is true)
            {
                if (SelectedScore is "Pass")
                {
                    _addedSubject.Passed = true;
                }
            }
            else
            {
                if (_addedSubject.Grade >= 1)
                {
                    _addedSubject.Passed = true;
                }
            }

            // Check if a user adds repeat without original sitting
            if (CheckOriginalSitting() is not false && DuplicateCheck() is not false)
            {
                _addedSubject.isRepeat = IsRepeat;
                _addedSubject = SetRepeatable(_addedSubject);

                // If not an ehnancement and repeat, then cap the score
                if (_addedSubject.isEnhancement is not true && _addedSubject.isRepeat)
                {
                    if (_addedSubject.Grade > 2.0m)
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
                    "A-",
                    "B+",
                    "B",
                    "B-",
                    "C+",
                    "C",
                    "C-",
                    "D+",
                    "D",
                    "E"
                };
                }
            }
        }
    }
}
