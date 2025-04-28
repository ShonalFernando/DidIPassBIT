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
            YearSummaries = new ObservableCollection<YearSummary>(EvaluateYearSummary());
            SemesterSummaries = new ObservableCollection<SemesterSummary>(EvaluateSemesterSummary());
        }

        // ============================ Flow Start
        private List<SemesterSummary> EvaluateSemesterSummary()
        {
            List<SemesterSummary> summaries = _subjectScores
            .GroupBy(s => new { s.Year, s.Semester })
            .Select(g =>
            {
                var subjects = g.ToList();

                var bestSubjects = subjects.GroupBy(s => s.SubjectCode).Select(g =>
                {
                    var bestAttempt = g.OrderByDescending(s => s.Grade).First();
                    return bestAttempt;
                }).ToList();

                // GPA Calculation: Sum(Weight) / Sum(Credits)
                decimal totalWeight = bestSubjects.Sum(s => s.Weight);
                decimal totalCredits = bestSubjects.Sum(s => s.Credit);
                decimal semesterGPA = totalCredits > 0 ? totalWeight / totalCredits : 0;

                // Earned Credits: Sum credits where Passed == true
                int earnedCredits = (int)bestSubjects.Where(s => s.Passed).Sum(s => s.Credit);

                // Enhancements Passed: all enhancement subjects passed
                bool enhancementsPassed = bestSubjects
                    .Where(s => s.isEnhancement)
                    .All(s => s.Passed);

                // Essentials Passed: all essential (non-enhancement) subjects passed
                bool essentialsPassed = bestSubjects
                    .Where(s => !s.isEnhancement)
                    .All(s => s.Passed);

                // Contains Repeats: any repeatable subject attempted
                bool containsRepeats = bestSubjects.Any(s => s.isRepeat);
                bool containsRepeatables = bestSubjects.Any(s => s.isRepeatable);

                // Semester Passed: Essentials must be passed
                bool semesterPassed = true;

                // Comments
                List<string> commentsPlus = new();
                List<string> commentsNeg = new();
                List<string> commentsWarn = new();

                // Check 1 : Enhancements
                if (g.Key.Semester is not 1 && g.Key.Semester is not 4)
                {
                    // No enhancement subjects
                    if (enhancementsPassed)
                    {
                        commentsPlus.Add("Enhancement subjects fully passed");
                    }
                    else
                    {
                        commentsNeg.Add("Enhancement subjects not fully passed");
                        semesterPassed = false;
                    }
                }
                else
                {
                    commentsPlus.Add($"No Enhancement subjects in semester {g.Key.Semester}");
                }

                // Check 2 : Essential Subjects
                if (essentialsPassed)
                {
                    commentsPlus.Add("Essential subjects fully passed");
                }
                else
                {
                    commentsNeg.Add("Essential subjects not fully passed");
                    semesterPassed = false;
                }

                foreach (var subject in bestSubjects)
                {
                    // If subject's Grade is LESS THAN 2.00 (i.e. below C)
                    // AND if it still has less than 3 attempts (meaning can retry)
                    if (subject.Grade < 2.00m && subject.Grade >= 1.00m && semesterPassed)
                    {
                        commentsWarn.Add($"Semester is passed but {subject.SubjectName} can be repeated next year to improve GPA if chances are available!");
                    }
                }

                if(!semesterPassed && g.Key.Semester %2 is not 0)
                {
                    commentsNeg.Add($"You can proceed to write all exams of semester {g.Key.Semester +1}, but cannot proceed to Year {g.Key.Year + 1}");
                }

                string title = $"Semester {g.Key.Semester} {(semesterPassed ? "Passed" : "Failed")}";
                string description = $"Accumilated Semester GPA → {Math.Round(semesterGPA, 2)} | Accumilated Credits → {earnedCredits}";

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
                    PositiveComments = commentsPlus,
                    NegativeComments = commentsNeg,
                    WarningComments = commentsWarn,
                    Title = title,
                    Description = description
                };
            })
                    .OrderBy(s => s.Year)
                    .ThenBy(s => s.Semester)
                    .ToList();

            return summaries;
        }

        private List<int> GetCompleteYears()
        {
            // Group all subjects by Semester
            var subjectsBySemester = _subjectScores
                .GroupBy(s => s.Semester)
                .ToDictionary(g => g.Key, g => g.ToList());

            // Helper: function to check if a semester is complete
            bool IsSemesterComplete(int semester)
            {
                if (!subjectsBySemester.ContainsKey(semester))
                    return false;

                var subjects = subjectsBySemester[semester];
                var distinctSubjects = subjects
                    .Where(s => !s.isRepeat)
                    .Select(s => s.SubjectCode)
                    .Distinct()
                    .ToList();

                int expectedSubjectCount = GetExpectedSubjectCountForSemester(semester);
                return distinctSubjects.Count == expectedSubjectCount;
            }

            List<int> completeYears = new List<int>();

            for (int year = 1; year <= 3; year++)
            {
                int sem1 = (year - 1) * 2 + 1;
                int sem2 = sem1 + 1;

                if (IsSemesterComplete(sem1) && IsSemesterComplete(sem2))
                {
                    completeYears.Add(year);
                }
            }

            return completeYears;
        }

        private List<YearSummary> EvaluateYearSummary()
        {
            var completedYears = GetCompleteYears();

            if (completedYears is null || completedYears.Count is 0)
            { 
                MessageBox.Show("No Completed Years", "Result Evaluation", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return new List<YearSummary>();
            }

                List<YearSummary> summaries = _subjectScores
            .GroupBy(s => new { s.Year })
            .Where(g => completedYears.Contains(g.Key.Year)) // 👈 Check if the year is completed
            .Select(g =>
            {
                var subjects = g.ToList();

                var bestSubjects = subjects.GroupBy(s => s.SubjectCode).Select(g => {
                    var bestAttempt = g.OrderByDescending(s => s.Grade).First();
                    return bestAttempt;
                }).ToList();

                // GPA Calculation: Sum(Weight) / Sum(Credits)
                decimal totalWeight = bestSubjects.Sum(s => s.Weight);
                decimal totalCredits = bestSubjects.Sum(s => s.Credit);
                decimal semesterGPA = totalCredits > 0 ? totalWeight / totalCredits : 0;

                // Earned Credits: Sum credits where Passed == true
                int earnedCredits = (int)bestSubjects.Where(s => s.Passed).Sum(s => s.Credit);

                // Earned Credits for C or above: Sum credits where Passed == true & above C
                int earnedCreditsCabove = (int)bestSubjects.Where(s => s.Passed && s.Grade >= 2.00m).Sum(s => s.Credit);

                // Enhancements Passed: all enhancement subjects passed
                bool enhancementsPassed = bestSubjects
                    .Where(s => s.isEnhancement)
                    .All(s => s.Passed);

                // Essentials Passed: all essential (non-enhancement) subjects passed
                bool essentialsPassed = bestSubjects
                    .Where(s => !s.isEnhancement)
                    .All(s => s.Passed);

                // Contains Repeats: any repeatable subject attempted
                bool containsRepeats = bestSubjects.Any(s => s.isRepeat);
                bool containsRepeatables = bestSubjects.Any(s => s.isRepeatable);

                // Semester Passed: Essentials must be passed
                bool semesterPassed = true;

                // Comments
                List<string> commentsPlus = new();
                List<string> commentsNeg = new();
                List<string> commentsWarn = new();

                if (!enhancementsPassed)
                    commentsNeg.Add("Enhancement subjects not fully passed");
                if (!essentialsPassed)
                    commentsNeg.Add("Essential subjects not fully passed");
                if (containsRepeats)
                    commentsWarn.Add("This semester contains repeated subjects");

                // Check 1 : Enhancements

                // No enhancement subjects
                if (enhancementsPassed)
                {
                    commentsPlus.Add("Enhancement subjects  fully passed");
                }
                else
                {
                    commentsNeg.Add("Enhancement subjects not fully passed");
                    semesterPassed = false;
                }

                // Check 2 : Essential Subjects
                if (essentialsPassed)
                {
                    commentsPlus.Add("Essential subjects fully passed");
                }
                else
                {
                    commentsNeg.Add("Essential subjects not fully passed");
                    semesterPassed = false;
                }

                // Check 3 : GPA
                if (Math.Round(semesterGPA, 2) >= 2.00m)
                {
                    commentsPlus.Add("Required GPA reached");
                }
                else
                {
                    commentsNeg.Add("Required GPA not reached");
                    semesterPassed = false;
                }

                // Check 4 : Credits for subjects atleast C
                if (earnedCreditsCabove >= 20)
                {
                    commentsPlus.Add("Sum of credits of subjects with atleast a C reached");
                }
                else
                {
                    commentsNeg.Add("Sum of credits of subjects with atleast a C not reached");
                    semesterPassed = false;
                }

                foreach (var subject in bestSubjects)
                {
                    // If subject's Grade is LESS THAN 2.00 (i.e. below C)
                    // AND if it still has less than 3 attempts (meaning can retry)
                    if (subject.Grade < 2.00m && subject.Grade >= 1.00m && semesterPassed)
                    {
                        commentsWarn.Add($"Year is passed but {subject.SubjectName} can be repeated to improve GPA if chances are available!");
                    }
                }

                string title = $"Year {g.Key.Year} {(semesterPassed ? "Passed" : "Failed")}";
                string description = $"Year GPA → {Math.Round(semesterGPA, 2)}| Credits → {earnedCredits} | Credits with C or Higher → {earnedCreditsCabove}";

                return new YearSummary
                {
                    Year = g.Key.Year,
                    YearGPA = Math.Round(semesterGPA, 2),
                    EarnedCredits = earnedCredits,
                    EarnedCreditsForCAbove = earnedCreditsCabove,
                    EnhancementsPassed = enhancementsPassed,
                    EssentialsPassed = essentialsPassed,
                    ContainsRepeats = containsRepeats,
                    YearPassed = semesterPassed,
                    PositiveComments = commentsPlus,
                    NegativeComments = commentsNeg,
                    WarningComments = commentsWarn,
                    Title = title,
                    Description = description
                };
            })
                    .OrderBy(s => s.Year)
                    .ToList();

            return summaries;
        }

    }
}
