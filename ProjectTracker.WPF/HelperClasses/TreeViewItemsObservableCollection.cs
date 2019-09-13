using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public class TreeViewItemsObservableCollection : ItemsChangeObservableCollection<TodoTreeViewItem>
    {
        protected override void item_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if(!(e.PropertyName.Equals("IsSelected") || e.PropertyName.Equals("IsExpanded")))
                base.item_PropertyChanged(sender, e);
        }
    }
}
