using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.Model
{
    public class GraduationCriteria
    {
        public bool PassedOverall { get; set; }  // Final result: Passed or Failed

        // Detail results
        public bool MinimumGpaMet { get; set; }
        public bool MinimumCreditsMet { get; set; }
        public bool PassEnhancementsMet { get; set; }
        public bool NoGradesBelowOneMet { get; set; }
        public bool NoGradesBelowDMet { get; set; }
        public bool SoftwareProjectMet { get; set; }
        public bool CCreditsPerLevelMet { get; set; }

        // Additional Information
        public List<string> Comments { get; set; } = new();

        // You can also add: Level (Year 1, Year 2, Degree)
        public int Level { get; set; }
    }
}
