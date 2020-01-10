using ProjectTracker.WPF.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace ProjectTracker.WPF.Views.Dialogs
{
    /// <summary>
    /// Interaction logic for OpenProjectDialog.xaml
    /// </summary>
    public partial class OpenProjectDialog : Window
    {
        public OpenProjectDialog(OpenProjectDialogViewModel viewModel)
        {
            InitializeComponent();

            DataContext = viewModel;
        }
    }
}
