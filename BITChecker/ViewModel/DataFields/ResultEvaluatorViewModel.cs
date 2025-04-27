using BITChecker.Helper;
using BITChecker.Model;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.ViewModel
{
    public partial class ResultEvaluatorViewModel : ViewModelBase
    {
        // Passed on Scores
        private List<SubjectScore> _subjectScores;

        //Summaries
        private ObservableCollection<SemesterSummary> _semesterSummaries;
        public ObservableCollection<SemesterSummary> SemesterSummaries
        {
            get => _semesterSummaries;
            set
            {
                _semesterSummaries = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<YearSummary> _yearSummaries;
        public ObservableCollection<YearSummary> YearSummaries
        {
            get => _yearSummaries;
            set
            {
                _yearSummaries = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<ResultEvaluation> _resultEvaluation;
        public ObservableCollection<ResultEvaluation> ResultEvaluation
        {
            get => _resultEvaluation;
            set
            {
                _resultEvaluation = value;
                OnPropertyChanged();
            }
        }
    }
}
