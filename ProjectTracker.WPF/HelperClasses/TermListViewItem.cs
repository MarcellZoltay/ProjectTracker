using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.HelperClasses
{
    public class TermListViewItem
    {
        public Term Term { get; }

        public ObservableCollection<Course> Courses { get; set; }

        public TermListViewItem(Term term)
        {
            Term = term;

            Courses = new ObservableCollection<Course>(Term.Courses);
        }

        public void AddCourse(Course course)
        {
            Term.AddCourse(course);
            Courses.Add(course);
        }
        public void RemoveCourse(Course course)
        {
            Term.RemoveCourse(course);
            Courses.Remove(course);
        }
    }
}
