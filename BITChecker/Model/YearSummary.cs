using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class YearSummary
    {
        public int Year { get; set; }

        public decimal YearGPA { get; set; }
        public int EarnedCredits { get; set; }
        public int EarnedCreditsForCAbove { get; set; }

        public bool EnhancementsPassed { get; set; }
        public bool EssentialsPassed { get; set; }
        public bool ContainsRepeats { get; set; }

        public bool YearPassed { get; set; }

        public List<string> PositiveComments { get; set; } = new();
        public List<string> NegativeComments { get; set; } = new();
        public List<string> WarningComments { get; set; } = new();

        //For Display
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        //Worry Meter
        public int WorryIndex { get; set; }
    }
}
