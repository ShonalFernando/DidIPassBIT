using PersofinDesktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BITChecker.ViewModel
{
    public partial class CalculatorViewModel : ViewModelBase
    {
        public ICommand AddSubjectCommand { get; }
        public ICommand GoToResultsCommand { get; }
        public ICommand EditSubjectCommand { get; }
        public ICommand UpdateSubjectCommand { get; }
        public ICommand DeleteSubjectCommand { get; }
        public ICommand CancelDeleteCommand { get; }

    }
}
