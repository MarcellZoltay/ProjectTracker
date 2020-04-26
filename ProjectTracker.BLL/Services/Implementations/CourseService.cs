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
        private IEventService eventService;

        public CourseService()
        {
            courseEntityService = UnityBootstrapper.Instance.Resolve<ICourseEntityService>();
            projectService = UnityBootstrapper.Instance.Resolve<IProjectService>();
            eventService = UnityBootstrapper.Instance.Resolve<IEventService>();
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

        public void ImportLessonEvent(Course course, DateTime startTime, DateTime endTime, string lessonType, string venue)
        {
            var events = course.Events;

            if (lessonType.Contains("E"))
            {
                var lecture = new Event($"Előadás - {venue}", startTime, endTime);
                events.Add(lecture);
                eventService.AddEventToProject(course.Project.Id, lecture);
            }
            else if (lessonType.Contains("G"))
            {
                var practice = new Event($"Gyakorlat - {venue}", startTime, endTime);
                events.Add(practice);
                eventService.AddEventToProject(course.Project.Id, practice);
            }
            else if (lessonType.Contains("L"))
            {
                var laboratory = new Event($"Labor - {venue}", startTime, endTime);
                events.Add(laboratory);
                eventService.AddEventToProject(course.Project.Id, laboratory);
            }
            else
            {
                var otherLesson = new Event($"Egyéb - {venue}", startTime, endTime);
                events.Add(otherLesson);
                eventService.AddEventToProject(course.Project.Id, otherLesson);
            }
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
