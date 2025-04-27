using BITChecker.Helper;
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
    public partial class ResultEvaluatorViewModel : ViewModelBase
    {
        public ResultEvaluatorViewModel(List<SubjectScore> subjectScores)
        {
            // Initialize Lists
            _subjectScores = subjectScores;
            _resultEvaluation = new();
            _yearSummaries = new();
            _semesterSummaries = new();

            _semesterSummaries.Clear();
            SemesterSummaries = new ObservableCollection<SemesterSummary>(EvaluatorWorkFlow());
            MessageBox.Show(SemesterSummaries.Count().ToString());
        }

        // ============================ Flow Start

        private List<SemesterSummary> EvaluatorWorkFlow()
        {
            List<SemesterSummary> summaries = _subjectScores
            .GroupBy(s => new { s.Year, s.Semester })
            .Select(g =>
            {
                var subjects = g.ToList();

                // GPA Calculation: Sum(Weight) / Sum(Credits)
                decimal totalWeight = subjects.Sum(s => s.Weight);
                decimal totalCredits = subjects.Sum(s => s.Credit);
                decimal semesterGPA = totalCredits > 0 ? totalWeight / totalCredits : 0;

                // Earned Credits: Sum credits where Passed == true
                int earnedCredits = (int)subjects.Where(s => s.Passed).Sum(s => s.Credit);

                // Enhancements Passed: all enhancement subjects passed
                bool enhancementsPassed = subjects
                    .Where(s => s.isEnhancement)
                    .All(s => s.Passed);

                // Essentials Passed: all essential (non-enhancement) subjects passed
                bool essentialsPassed = subjects
                    .Where(s => !s.isEnhancement)
                    .All(s => s.Passed);

                // Contains Repeats: any repeatable subject attempted
                bool containsRepeats = subjects.Any(s => s.isRepeat);

                // Semester Passed: Essentials must be passed
                bool semesterPassed = essentialsPassed;

                // Comments
                List<string> comments = new();
                if (!enhancementsPassed)
                    comments.Add("Enhancement subjects not fully passed");
                if (!essentialsPassed)
                    comments.Add("Essential subjects not fully passed");
                if (containsRepeats)
                    comments.Add("This semester contains repeated subjects");

                comments.Add("Enhancement subjects are fully passed");
                comments.Add("Essential subjects are fully passed");
                comments.Add("Credits with atleast C reached");
                comments.Add("Credits reached");
                comments.Add("All Esential subjects > 1.00");
                comments.Add("No issues");

                string title = $"Semester {g.Key.Semester} {(semesterPassed ? "Passed" : "Failed")}";
                string description = $"Semester GPA → {Math.Round(semesterGPA, 2)}| Credits → {earnedCredits} | Credits with C or Higher → 22";

                return new SemesterSummary
                {
                    Year = g.Key.Year,
                    Semester = g.Key.Semester,
                    SemesterGPA = Math.Round(semesterGPA, 2),
                    EarnedCredits = earnedCredits,
                    EnhancementsPassed = enhancementsPassed,
                    EssentialsPassed = essentialsPassed,
                    ContainsRepeats = containsRepeats,
                    SemesterPassed = semesterPassed,
                    Comments = comments,
                    Title = title,
                    Description = description
                };
                    })
                    .OrderBy(s => s.Year)
                    .ThenBy(s => s.Semester)
                    .ToList();

            return summaries;
        }

    }
}
