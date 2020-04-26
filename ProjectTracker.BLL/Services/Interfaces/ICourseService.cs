using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface ICourseService
    {
        List<Course> GetCoursesByTermId(int termId);
        Course CreateCourse(int termId, string courseTitle, int credit);
        void UpdateCourse(Course courseToUpdate);
        Task UpdateCourseAsync(Course courseToUpdate);
        void DeleteCourse(Course courseToDelete);
        Task DeleteCourseAsync(Course courseToDelete);
        void ImportLessonEvent(Course course, DateTime startTime, DateTime endTime, string lessonType, string venue);
    }
}
