using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Implementations
{
    public class CourseService : ICourseService, IModelEntityMapper<Course, CourseEntity>
    {
        private ICourseEntityService courseEntityService;
        private IProjectService projectService;

        public CourseService()
        {
            courseEntityService = UnityBootstrapper.Instance.Resolve<ICourseEntityService>();
            projectService = UnityBootstrapper.Instance.Resolve<IProjectService>();
        }

        public List<Course> GetCoursesByTermId(int termId)
        {
            List<Course> courses = new List<Course>();
            var courseEntities = courseEntityService.GetCoursesByTermId(termId);

            foreach (var item in courseEntities)
            {
                var course = ConvertToModel(item);

                var project = projectService.GetProjectByCourseId(course.Id);
                course.Project = project;

                courses.Add(course);
            }

            return courses;
        }

        public Course CreateCourse(int termId, string courseTitle, int credit)
        {
            var course = new Course(courseTitle) { Credit = credit };

            var courseEntity = ConvertToEntity(course);
            courseEntity.TermID = termId;

            course.Id = courseEntityService.AddCourse(courseEntity);

            var project = projectService.CreateProject(courseTitle, course.Id);
            course.Project = project;

            return course;
        }

        public void UpdateCourse(Course courseToUpdate)
        {
            var courseEntity = ConvertToEntity(courseToUpdate);

            courseEntityService.UpdateCourse(courseEntity);

            projectService.UpdateProject(courseToUpdate.Project);
        }
        public async Task UpdateCourseAsync(Course courseToUpdate)
        {
            await Task.Run(() => UpdateCourse(courseToUpdate));
        }

        public void DeleteCourse(Course courseToDelete)
        {
            projectService.DeleteProject(courseToDelete.Project);

            var courseEntity = ConvertToEntity(courseToDelete);
            courseEntityService.DeleteCourse(courseEntity);
        }
        public async Task DeleteCourseAsync(Course courseToDelete)
        {
            await Task.Run(() => DeleteCourse(courseToDelete));
        }
        

        public Course ConvertToModel(CourseEntity entity)
        {
            return new Course(entity.Title)
            {
                Id = entity.Id,
                Credit = entity.Credit,
                IsFulfilled = entity.IsFulfilled
            };
        }

        public CourseEntity ConvertToEntity(Course model)
        {
            return new CourseEntity()
            {
                Id = model.Id,
                Title = model.Title,
                Credit = model.Credit,
                IsFulfilled = model.IsFulfilled
            };
        }
    }
}
