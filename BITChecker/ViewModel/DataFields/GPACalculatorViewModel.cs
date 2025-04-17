using BITChecker.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.ViewModel
{
    public partial class GPACalculatorViewModel : ViewModelBase
    {
        public ObservableCollection<int> AvailableYears { get; set; } = new();
        public ObservableCollection<Subject> AvailableSubjects { get; set; } = new();
        public ObservableCollection<string> AvailableGrades { get; set; } = new();
        public ObservableCollection<PerSubjectScore> SelectedScores { get; set; } = new();

        private int _selectedYear;
        public int SelectedYear
        {
            get => _selectedYear;
            set
            {
                if (_selectedYear != value)
                {
                    _selectedYear = value;
                    OnPropertyChanged();
                }
            }
        }

        private Subject? _selectedSubject;
        public Subject? SelectedSubject
        {
            get => _selectedSubject;
            set
            {
                if (_selectedSubject != value)
                {
                    _selectedSubject = value;
                    OnPropertyChanged();
                }
            }
        }

        private string? _selectedGrade;
        public string? SelectedGrade
        {
            get => _selectedGrade;
            set
            {
                if (_selectedGrade != value)
                {
                    _selectedGrade = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
