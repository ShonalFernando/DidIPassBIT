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

        public decimal SemesterGPA { get; set; }
        public int EarnedCredits { get; set; }

        // Criteria
        public bool EnhancementsPassed { get; set; }
        public bool EssentialsPassed { get; set; } // D or above
        public bool MinimumGpaMet { get; set; }       // 2.0
        public bool MinimumCreditsMet { get; set; }   // 30
        public bool SoftwareProjectMet { get; set; }  // check for L3
        public bool CCreditsPerLevelMet { get; set; } // 20 with atleast C


        // Other Data
        public bool ContainsRepeats { get; set; }
        public bool SemesterPassed { get; set; }

        public List<string> Comments { get; set; } = new();
    }
}
