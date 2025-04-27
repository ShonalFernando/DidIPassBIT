using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class SemesterSummary
    {
        public int Year { get; set; }
        public int Semester { get; set; }

        public decimal SemesterGPA { get; set; }
        public int EarnedCredits { get; set; }

        public bool EnhancementsPassed { get; set; }
        public bool EssentialsPassed { get; set; }
        public bool ContainsRepeats { get; set; }

        public bool SemesterPassed { get; set; }

        public List<string> Comments { get; set; } = new();

        //For Display
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
