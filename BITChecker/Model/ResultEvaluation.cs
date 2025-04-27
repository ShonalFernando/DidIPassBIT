using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Helper
{
    public class ResultEvaluation
    {
        // Sem Info
        public int Semester { get; set; }
        public int Year { get; set; }

        // Display Elements
        public DisplayIcon DisplayIcon { get;set; }

        // Results
        public bool isSemPassed { get; set; }

        // Calculation Data
        public decimal SemGPA { get; set; }
        public decimal SemCredits { get; set; }

        // Comments
        public List<string> Comments { get; set; } = new List<string>();
    }


    public enum DisplayIcon
    {
        Fail,
        Warning,
        Pass
    }
}
