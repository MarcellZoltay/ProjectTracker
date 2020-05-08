using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Term : Bindable
    {
        public int Id { get; set; }

        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public List<Course> Courses { get; }

        private int totalCredit;
        public int TotalCredit
        {
            get { return totalCredit; }
            set { SetProperty(ref totalCredit, value); }
        }

        private int totalFulfilledCredit;
        public int TotalFulfilledCredit
        {
            get { return totalFulfilledCredit; }
            set { SetProperty(ref totalFulfilledCredit, value); }
        }

        public Term(string title)
        {
            Title = title;

            Courses = new List<Course>();
        }

        public void AddCourse(Course course)
        {
            Courses.Add(course);

            TotalCredit += course.Credit;

            if (course.IsFulfilled)
                TotalFulfilledCredit += course.Credit;

            course.PropertyChanged += Course_PropertyChanged;
        }
        public void AddCourseRange(IEnumerable<Course> courses)
        {
            foreach (var course in courses)
            {
                AddCourse(course);
            }
        }
        public void RemoveCourse(Course course)
        {
            Courses.Remove(course);

            TotalCredit -= course.Credit;

            if (course.IsFulfilled)
                TotalFulfilledCredit -= course.Credit;

            course.PropertyChanged -= Course_PropertyChanged;
        }

        private void Course_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsFulfilled")
            {
                var course = sender as Course;

                if (course.IsFulfilled)
                    TotalFulfilledCredit += course.Credit;
                else
                    TotalFulfilledCredit -= course.Credit;
            }
            else if (e.PropertyName == "Credit")
            {
                TotalCredit = 0;
                TotalFulfilledCredit = 0;

                foreach (var course in Courses)
                {
                    TotalCredit += course.Credit;

                    if (course.IsFulfilled)
                        TotalFulfilledCredit += course.Credit;
                }
            }
        }
    }
}
