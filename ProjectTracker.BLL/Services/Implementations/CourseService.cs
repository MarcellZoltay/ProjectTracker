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
        private ITodoService todoService;

        public CourseService()
        {
            courseEntityService = UnityBootstrapper.Instance.Resolve<ICourseEntityService>();
            projectService = UnityBootstrapper.Instance.Resolve<IProjectService>();
            todoService = UnityBootstrapper.Instance.Resolve<ITodoService>();
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

        public void ImportLessonTodo(Course course, DateTime startDate, string lessonType, string venue)
        {
            var todos = course.Todos;

            if (lessonType.Contains("E"))
            {
                Todo lecturesTodo = todos.Where(t => t.Text.Contains($"Előadások - {venue}")).FirstOrDefault();
                if (lecturesTodo == null)
                {
                    lecturesTodo = new Todo($"Előadások - {venue}");
                    todos.Add(lecturesTodo);

                    todoService.AddTodoToProject(course.Project.Id, lecturesTodo);
                }

                var lecture = new Todo("Előadás") { Deadline = startDate };
                lecturesTodo.AddTodo(lecture);

                todoService.AddSubTodo(lecturesTodo.Id, lecture);
            }
            else if (lessonType.Contains("G"))
            {
                Todo practicesTodo = todos.Where(t => t.Text.Contains($"Gyakorlatok - {venue}")).FirstOrDefault();
                if (practicesTodo == null)
                {
                    practicesTodo = new Todo($"Gyakorlatok - {venue}");
                    todos.Add(practicesTodo);

                    todoService.AddTodoToProject(course.Project.Id, practicesTodo);
                }

                var practice = new Todo("Gyakorlat") { Deadline = startDate };
                practicesTodo.AddTodo(practice);

                todoService.AddSubTodo(practicesTodo.Id, practice);
            }
            else if (lessonType.Contains("L"))
            {
                Todo laboratoriesTodo = todos.Where(t => t.Text.Contains($"Laborok - {venue}")).FirstOrDefault();
                if (laboratoriesTodo == null)
                {
                    laboratoriesTodo = new Todo($"Laborok - {venue}");
                    todos.Add(laboratoriesTodo);

                    todoService.AddTodoToProject(course.Project.Id, laboratoriesTodo);
                }

                var laboratory = new Todo("Labor") { Deadline = startDate };
                laboratoriesTodo.AddTodo(laboratory);

                todoService.AddSubTodo(laboratoriesTodo.Id, laboratory);
            }
            else
            {
                Todo otherLessonsTodo = todos.Where(t => t.Text.Contains($"Egyéb órák - {venue}")).FirstOrDefault();
                if (otherLessonsTodo == null)
                {
                    otherLessonsTodo = new Todo($"Egyéb órák - {venue}");
                    todos.Add(otherLessonsTodo);

                    todoService.AddTodoToProject(course.Project.Id, otherLessonsTodo);
                }

                var otherLessons = new Todo("Egyéb") { Deadline = startDate };
                otherLessonsTodo.AddTodo(otherLessons);

                todoService.AddSubTodo(otherLessonsTodo.Id, otherLessons);
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
