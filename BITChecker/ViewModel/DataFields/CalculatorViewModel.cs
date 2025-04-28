using BITChecker.Model;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        private int _filterYear;
        public int FilterYear
        {
            get => _filterYear;
            set
            {
                _filterYear = value;
                OnPropertyChanged();
                AdjustSemesters();
                FilterSort();
            }
        }

        private int _filterSemester;
        public int FilterSemester
        {
            get => _filterSemester;
            set
            {
                _filterSemester = value;
                OnPropertyChanged();
                FilterSort();
            }
        }

        private Subject _selectedSubject;
        public Subject SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                _selectedSubject = value;
                OnPropertyChanged();
                PostSubjectSelect();
            }
        }

        private int _examYear;
        public int ExamYear
        {
            get => _examYear;
            set
            {
                _examYear = value;
                OnPropertyChanged();
            }
        }

        private string _selectedScore = string.Empty;
        public string SelectedScore
        {
            get => _selectedScore;
            set
            {
                _selectedScore = value;
                OnPropertyChanged();
            }
        }

        private string _updatedScore = string.Empty;
        public string UpdatedScore
        {
            get => _updatedScore;
            set
            {
                _updatedScore = value;
                OnPropertyChanged();
            }
        }

        private bool _isRepeat;
        public bool IsRepeat
        {
            get => _isRepeat;
            set
            {
                _isRepeat = value;
                OnPropertyChanged();
            }
        }

        private SubjectScore _selectedSubjectScore;
        public SubjectScore SelectedSubjectScore
        {
            get => _selectedSubjectScore;
            set
            {
                _selectedSubjectScore = value;
                OnPropertyChanged();
            }
        }

        // Collections
        private ObservableCollection<Subject> _sortedSubjects;
        public ObservableCollection<Subject> SortedSubjects
        {
            get => _sortedSubjects;
            set
            {
                _sortedSubjects = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> _semestersList;
        public ObservableCollection<int> SemestersList
        {
            get => _semestersList;
            set
            {
                _semestersList = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<int> _yearList;
        public ObservableCollection<int> YearList
        {
            get => _yearList;
        }

        private ObservableCollection<string> _gradeOptions;
        public ObservableCollection<string> GradeOptions
        {
            get => _gradeOptions;
            set
            {
                _gradeOptions = value;
                OnPropertyChanged();
            }
        }


        private ObservableCollection<SubjectScore> _subjectScores;
        public ObservableCollection<SubjectScore> SubjectScores
        {
            get => _subjectScores;
            set
            {
                _subjectScores = value;
                OnPropertyChanged();
            }
        }

        // COUNTDOWN
        private int _countdownValue;
        private double _progressValue;
        private readonly DispatcherTimer _timer;
        private readonly int _totalSeconds = 3;

        public int CountdownValue
        {
            get => _countdownValue;
            set
            {
                _countdownValue = value;
                OnPropertyChanged();
            }
        }

        public double ProgressValue
        {
            get => _progressValue;
            set
            {
                _progressValue = value;
                OnPropertyChanged();
            }
        }
    }
}
