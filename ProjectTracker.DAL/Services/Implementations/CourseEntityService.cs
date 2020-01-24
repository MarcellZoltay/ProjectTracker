using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Implementations
{
    public class CourseEntityService : ICourseEntityService
    {
        public List<CourseEntity> GetCoursesByTermId(int termId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from c in db.Courses.AsNoTracking()
                        where c.TermID == termId
                        select c).ToList();
            }
        }

        public int AddCourse(CourseEntity course)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Courses.Add(course);
                db.SaveChanges();
            }

            return course.Id;
        }

        public void UpdateCourse(CourseEntity courseToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                var entity = (from c in db.Courses
                              where c.Id == courseToUpdate.Id
                              select c).First();

                courseToUpdate.TermID = entity.TermID;

                db.Entry(entity).State = EntityState.Detached;
                db.Entry(courseToUpdate).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void DeleteCourse(CourseEntity courseToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(courseToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
