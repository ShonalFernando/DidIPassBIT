﻿using BITChecker.Model;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        private bool CanAddSubject(object? parameter)
        {
            if (SelectedSubject is null)
                return false;
            if (SelectedScore is null)
                return false;
            return true;
        }

        private bool CanNavigate()
        {
            var subjectsBySemester = SubjectScores
                .GroupBy(s => s.Semester)
                .ToDictionary(g => g.Key, g => g.ToList());

            List<int> completeSemesters = new List<int>();
            List<int> incompleteSemesters = new List<int>();

            foreach (var kvp in subjectsBySemester)
            {
                int semester = kvp.Key;
                var subjects = kvp.Value;

                var distinctSubjects = subjects
                    .Where(s => !s.isRepeat)
                    .Select(s => s.SubjectCode)
                    .Distinct()
                    .ToList();

                int expectedSubjectCount = GetExpectedSubjectCountForSemester(semester);

                if (distinctSubjects.Count == expectedSubjectCount)
                {
                    completeSemesters.Add(semester);
                }
                else
                {
                    incompleteSemesters.Add(semester);
                }
            }

            if (completeSemesters.Any())
            {
                if (incompleteSemesters.Any())
                {
                    string message = $"Semesters {string.Join(", ", incompleteSemesters)} have missing subjects.\n" +
                                     $"Subjects from these semesters will be removed.\n\nDo you want to continue?";
                    var result = MessageBox.Show(message, "Incomplete Semesters Detected", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                    if (result != MessageBoxResult.Yes)
                        return false; // User cancelled

                    // Remove subjects of incomplete semesters
                    foreach (var subject in SubjectScores.Where(s => incompleteSemesters.Contains(s.Semester)).ToList())
                    {
                        SubjectScores.Remove(subject);
                    }
                }

                return true; // At least one complete semester
            }
            else
            {
                string message = "No semester is fully completed.\nPlease complete the subjects for at least one semester.";
                MessageBox.Show(message, "Missing Subjects Detected", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private bool CanNavigateToResult(object? parameter)
        {
            if (SubjectScores is not null)
            {
                return (SubjectScores.Count > 0);
            }

            return true;
        }

        // Can user delete or Edit Subject
        private bool CanEditorDeleteSubject(object? parameter)
        {
            return (SelectedSubjectScore is not null);
        }

        // Check if user adds repeat without original sitting
        private bool CheckOriginalSitting()
        {
            // Check if SelectedSubject is already entered (non-repeat)
            bool alreadyAdded = SubjectScores.Any(s =>
                s.SubjectID == SelectedSubject.Id && !s.isRepeat);

            if (IsRepeat && !alreadyAdded)
            {
                MessageBox.Show("Cannot add a repeat exam without first entering the original attempt!",
                                "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false; // Stop here
            }

            return true;
        }

        // Check if All Subjects are Added per semester and is not repeat (min)
        private void ValidateSubjectScores(ObservableCollection<SubjectScore> enteredScores,
            ObservableCollection<Subject> allSubjects)
        {
            // Group entered subjects by (Year, Semester)
            var enteredGroups = enteredScores
                .GroupBy(s => new { s.Year, s.Semester })
                .ToDictionary(g => g.Key, g => g.Select(s => s.SubjectID).ToHashSet());

            // Group available subjects by (Year, Semester)
            var allGroups = allSubjects
                .GroupBy(s => new { s.Year, s.Semester })
                .ToDictionary(g => g.Key, g => g.Select(s => s.Id).ToHashSet());

            foreach (var allGroup in allGroups)
            {
                var year = allGroup.Key.Year;
                var semester = allGroup.Key.Semester;
                var allSubjectIds = allGroup.Value;

                if (enteredGroups.TryGetValue(allGroup.Key, out var enteredSubjectIds))
                {
                    // Check if all subjects for this semester are entered
                    if (!allSubjectIds.IsSubsetOf(enteredSubjectIds))
                    {
                        MessageBox.Show($"Error: Some subjects missing in Year {year} Semester {semester}",
                                        "Incomplete Semester", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
            }

            // If no error detected
            MessageBox.Show("All subjects are entered correctly!",
                            "Validation Successful", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        // Check if user tries to add sub
        private bool DuplicateCheck()
        {
            // How many times this SubjectID already exists
            int attemptCount = SubjectScores.Count(s => s.SubjectID == SelectedSubject.Id);

            bool originalExists = SubjectScores.Any(s =>
                s.SubjectID == SelectedSubject.Id && !s.isRepeat);

            // Find the latest (last entered) SubjectScore for this subject
            var lastAttempt = SubjectScores
                .Where(s => s.SubjectID == SelectedSubject.Id)
                .OrderByDescending(s => s.isRepeat) // Prefer repeats first
                .FirstOrDefault();

        check1:
            if (IsRepeat)
            {
                
                if (!originalExists)
                {
                    MessageBox.Show("Cannot add a repeat attempt without entering the original attempt first!",
                                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsRepeat = false;
                    return false;
                }

                if (attemptCount >= 3)
                {
                    MessageBox.Show("You have already added maximum allowed attempts (Original + 2 Repeats).",
                                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsRepeat = false;
                    return false;
                }
            }
            else // Not Repeat
            {
                if (attemptCount > 0)
                {
                    var result = MessageBox.Show(
                        "You have already entered this subject. Do you want to add it as a repeat exam?",
                        "Duplicate Subject",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        IsRepeat = true;
                        goto check1;
                    }
                    else
                    {
                        return false;
                    }
                }
            }


            if (lastAttempt != null)
            {
                if (!lastAttempt.isRepeatable)
                {
                    MessageBox.Show("This subject is not repeatable based on last score!",
                                    "Repeat Not Allowed", MessageBoxButton.OK, MessageBoxImage.Warning);
                    IsRepeat = false;
                    return false;
                }
            }
            return true; // Passed all checks
        }
    }
}
