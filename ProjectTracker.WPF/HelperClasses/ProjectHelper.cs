using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public static class ProjectHelper
    {
        public static IEnumerable<ProjectExpandable> ConvertToProjectExpandables(this IEnumerable<Project> projects)
        {
            var result = new List<ProjectExpandable>();

            foreach (var item in projects)
            {
                result.Add(new ProjectExpandable(item));
            }

            return result;
        }
    }
}
