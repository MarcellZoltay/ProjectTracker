using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectTracker.WPF.Views
{
    /// <summary>
    /// Interaction logic for StartPage
    /// </summary>
    public partial class StartPage : UserControl
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void LvProjects_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            HitTestResult r = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            if (r.VisualHit.GetType() != typeof(ListViewItem))
                lvProjects.UnselectAll();
        }
    }
}
