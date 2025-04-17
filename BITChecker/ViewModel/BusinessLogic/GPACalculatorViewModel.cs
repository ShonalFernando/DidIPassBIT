using BITChecker.Data;
using BITChecker.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.ViewModel
{
    public partial class GPACalculatorViewModel : ViewModelBase
    {
        private readonly IDataRepository<Subject> _subjectRepository;

        public GPACalculatorViewModel(IDataRepository<Subject> repository)
        {
            _subjectRepository = new SqliteDataRepository<Subject>(new AppDBContext());


            AddScoreCommand = new RelayCommand(AddScore, CanAddScore);

            // Initialize available years from subjects
            var allSubjects = _repository.GetAllSubjects();
            var years = allSubjects.Select(s => s.Semester).Distinct().OrderBy(y => y);
            foreach (var y in years)
                AvailableYears.Add(y);
        }

        private void LoadSubjectsForYear()
        {
            AvailableSubjects.Clear();
            var subjects = _repository.GetAllSubjects()
                .Where(s => s.Semester == SelectedYear);

            foreach (var subject in subjects)
                AvailableSubjects.Add(subject);
        }

        private void UpdateGradeOptions()
        {
            AvailableGrades.Clear();

            if (SelectedSubject?.Code.StartsWith("EN") == true)
            {
                AvailableGrades.Add("Pass");
                AvailableGrades.Add("Fail");
            }
            else
            {
                AvailableGrades.Add("A+");
                AvailableGrades.Add("A");
                AvailableGrades.Add("A-");
                AvailableGrades.Add("B+");
                AvailableGrades.Add("B");
                AvailableGrades.Add("B-");
                AvailableGrades.Add("C+");
                AvailableGrades.Add("C");
                AvailableGrades.Add("C-");
                AvailableGrades.Add("D+");
                AvailableGrades.Add("D");
                AvailableGrades.Add("E");
            }
        }

        private bool CanAddScore()
        {
            return SelectedSubject != null && !string.IsNullOrEmpty(SelectedGrade);
        }

        private void AddScore()
        {
            if (SelectedSubject == null || SelectedGrade == null)
                return;

            var score = new PerSubjectScore
            {
                Subject = SelectedSubject,
                PointValue = GetPointValue(SelectedSubject, SelectedGrade)
            };

            score.QaulityPoint = score.PointValue * SelectedSubject.GPACredit;
            SelectedScores.Add(score);
        }

        private decimal GetPointValue(Subject subject, string grade)
        {
            if (subject.Code.StartsWith("EN"))
            {
                // Pass = 1.0, Fail = 0.0 — You can adjust this if you treat it differently
                return grade == "Pass" ? 1.0m : 0.0m;
            }

            // GPA scale
            return grade switch
            {
                "A+" => 4.0m,
                "A" => 4.0m,
                "A-" => 3.7m,
                "B+" => 3.3m,
                "B" => 3.0m,
                "B-" => 2.7m,
                "C+" => 2.3m,
                "C" => 2.0m,
                "C-" => 1.7m,
                "D+" => 1.3m,
                "D" => 1.0m,
                "E" => 0.0m,
                _ => 0.0m
            };
        }
    }
}
