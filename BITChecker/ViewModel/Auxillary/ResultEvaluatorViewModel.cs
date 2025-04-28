using BITChecker.Helper;
using BITChecker.Model;
using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BITChecker.ViewModel
{
    public partial class ResultEvaluatorViewModel : ViewModelBase
    {
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
