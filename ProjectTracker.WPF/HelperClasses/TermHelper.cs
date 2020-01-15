using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public static class TermHelper
    {
        public static IEnumerable<TermListViewItem> ConvertToTermListViewItems(this IEnumerable<Term> terms)
        {
            var result = new List<TermListViewItem>();

            foreach (var item in terms)
            {
                result.Add(new TermListViewItem(item));
            }

            return result;
        }
    }
}
