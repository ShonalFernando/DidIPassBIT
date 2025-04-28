using BITChecker.Model;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        // Check if a subject can be repeated!
        private SubjectScore SetRepeatable(SubjectScore subject)
        {
            // Count how many times this SubjectID has been attempted
            int attemptCount = SubjectScores.Count(s => s.SubjectID == subject.SubjectID);

            if (subject.isEnhancement)
            {
                // Enhancement subjects: Can repeat if failed
                subject.isRepeatable = !subject.Passed;
            }
            else
            {
                // Normal subjects: Can repeat if below C (C = 2.0)
                subject.isRepeatable = subject.Grade < 2.0m;
            }

            // If it's the 3rd attempt, stop here (max allowed attempts = 3)
            if (attemptCount >= 2)
            {
                subject.isRepeatable = false;
            }
            return subject;
        }

        private void YearNormalizeWarning()
        {
            if (ExamYear < 2000)
            {
                MessageBox.Show($"BIT was not even started yet at {ExamYear}! \n" +
                    $"Year is only noted for further information and is not validated",
                                "Exam Year", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else if (ExamYear > DateTime.Now.Year)
            {
                MessageBox.Show($"How did you complete an exam in the future? \n" +
                    $"Year is only noted for further information and is not validated",
                                "Exam Year", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        // Helper function
        private int GetExpectedSubjectCountForSemester(int semester)
        {
            // You can hardcode or later load from DB or config
            return semester switch
            {
                1 => 5,
                2 => 5,
                3 => 5,
                4 => 5,
                5 => 4,
                6 => 4,
                _ => 5, // Default
            };
        }
    }
}
