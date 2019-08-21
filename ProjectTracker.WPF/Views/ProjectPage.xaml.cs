using ProjectTracker.WPF.HelperClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ProjectTracker.WPF.Views
{
    /// <summary>
    /// Interaction logic for ProjectPage
    /// </summary>
    public partial class ProjectPage : UserControl
    {
        public ProjectPage()
        {
            InitializeComponent();
        }

        private void TvTodos_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = (TodoTreeViewItem)tvTodos.SelectedItem;
            if(selectedItem != null)
            {
                selectedItem.IsSelected = false;
            }

            HitTestResult result = VisualTreeHelper.HitTest(this, e.GetPosition(this));

            var element = result.VisualHit;
            while (element != null && !(element is TreeViewItem))
                element = VisualTreeHelper.GetParent(element);

            if (element is TreeViewItem)
            {
                TreeViewItem item = (TreeViewItem)element;

                ((TodoTreeViewItem)item.DataContext).IsSelected = true;

                e.Handled = true;
            }
        }

        private void MainGrid_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.Handled)
                return;

            var selectedItem = (TodoTreeViewItem)tvTodos.SelectedItem;
            if (selectedItem != null)
            {
                selectedItem.IsSelected = false;
            }

            lvWebpageLinks.UnselectAll();
            lvFiles.UnselectAll();
            lvFolders.UnselectAll();
            lvApps.UnselectAll();
        }
    }
}
