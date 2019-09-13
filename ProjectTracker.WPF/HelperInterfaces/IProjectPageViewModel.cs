using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperInterfaces
{
    public interface IProjectPageViewModel
    {
        void OpenPaths(IList selectedItems);
        void OpenWebpageLink(IList selectedItems);
        void DeletePaths(IList selectedItems);
    }
}
