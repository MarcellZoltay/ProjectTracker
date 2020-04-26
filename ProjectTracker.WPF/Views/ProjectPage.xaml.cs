using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.HelperInterfaces;
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
            if (selectedItem != null)
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

            lvDeadlines.UnselectAll();
            lvEvents.UnselectAll();
            lvWebpageLinks.UnselectAll();
            lvFiles.UnselectAll();
            lvFolders.UnselectAll();
            lvApps.UnselectAll();
        }

        private void LvWebpageLinks_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((IProjectPageViewModel)DataContext).OpenWebpageLinks(lvWebpageLinks.SelectedItems);
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                ((IProjectPageViewModel)DataContext).DeletePaths(lvWebpageLinks.SelectedItems);
            }
        }
        private void LvWebpageLinks_OpenContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).OpenWebpageLinks(lvWebpageLinks.SelectedItems);
        }
        private void LvWebpageLinks_DeleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).DeletePaths(lvWebpageLinks.SelectedItems);
        }

        private void LvFiles_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((IProjectPageViewModel)DataContext).OpenPaths(lvFiles.SelectedItems);
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                ((IProjectPageViewModel)DataContext).DeletePaths(lvFiles.SelectedItems);
            }
        }
        private void LvFiles_OpenContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).OpenPaths(lvFiles.SelectedItems);
        }
        private void LvFiles_DeleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).DeletePaths(lvFiles.SelectedItems);
        }

        private void LvFolders_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((IProjectPageViewModel)DataContext).OpenPaths(lvFolders.SelectedItems);
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                ((IProjectPageViewModel)DataContext).DeletePaths(lvFolders.SelectedItems);
            }
        }
        private void LvFolders_OpenContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).OpenPaths(lvFolders.SelectedItems);
        }
        private void LvFolders_DeleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).DeletePaths(lvFolders.SelectedItems);
        }

        private void LvApps_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                ((IProjectPageViewModel)DataContext).OpenPaths(lvApps.SelectedItems);
            }
            else if (e.Key == System.Windows.Input.Key.Delete)
            {
                ((IProjectPageViewModel)DataContext).DeletePaths(lvApps.SelectedItems);
            }
        }
        private void LvApps_OpenContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).OpenPaths(lvApps.SelectedItems);
        }
        private void LvApps_DeleteContextMenuItem_Click(object sender, RoutedEventArgs e)
        {
            ((IProjectPageViewModel)DataContext).DeletePaths(lvApps.SelectedItems);
        }

    }
}
