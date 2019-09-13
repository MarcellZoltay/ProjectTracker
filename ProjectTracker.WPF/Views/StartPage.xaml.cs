using ProjectTracker.WPF.HelperClasses;
using System.Windows;
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

        private void TvTodos_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedItem = (TodoTreeViewItem)((TreeView)sender).SelectedItem;
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

            HitTestResult result = VisualTreeHelper.HitTest(this, e.GetPosition(this));
            var element = result.VisualHit;

            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(element); i++)
            {
                var child = VisualTreeHelper.GetChild(element, i);

                var treeView = GetVisualChild<TreeView>(child);
            }

            //var selectedItem = (TodoTreeViewItem)tvTodos.SelectedItem;
            //if (selectedItem != null)
            //{
            //    selectedItem.IsSelected = false;
            //}

            lvProjects.UnselectAll();
        }

        private T GetVisualChild<T>(DependencyObject parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }

        private void TvTodos_PreviewMouseWheel(object sender, System.Windows.Input.MouseWheelEventArgs e)
        {
            if (!e.Handled)
            {
                e.Handled = true;
                var eventArg = new System.Windows.Input.MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                var parent = ((Control)sender).Parent as UIElement;
                parent.RaiseEvent(eventArg);
            }
        }
    }
}
