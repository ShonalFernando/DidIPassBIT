using BITChecker.Model;
using BITChecker.ViewModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BITChecker.View
{
    /// <summary>
    /// Interaction logic for ResultView.xaml
    /// </summary>
    public partial class ResultView : Page
    {
        public ResultView(ObservableCollection<SubjectScore> subjectScores)
        {
            InitializeComponent();
            DataContext = new ResultEvaluatorViewModel(subjectScores.ToList());
        }
    }
}
