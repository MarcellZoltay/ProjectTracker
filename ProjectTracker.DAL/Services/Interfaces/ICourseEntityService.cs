using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Interfaces
{
    public interface ICourseEntityService
    {
        List<CourseEntity> GetCoursesByTermId(int termId);
        int AddCourse(CourseEntity course);
        void UpdateCourse(CourseEntity courseToUpdate);
        void DeleteCourse(CourseEntity courseToDelete);
    }
}
