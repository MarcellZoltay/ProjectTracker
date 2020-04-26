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
    public class ProjectService : IProjectService, IModelEntityMapper<Project, ProjectEntity>
    {
        private IProjectEntityService projectEntityService;
        private IDeadlineService deadlineService;
        private IEventService eventService;
        private ITodoService todoService;
        private IPathService pathService;

        public ProjectService()
        {
            projectEntityService = UnityBootstrapper.Instance.Resolve<IProjectEntityService>();
            deadlineService = UnityBootstrapper.Instance.Resolve<IDeadlineService>();
            eventService = UnityBootstrapper.Instance.Resolve<IEventService>();
            todoService = UnityBootstrapper.Instance.Resolve<ITodoService>();
            pathService = UnityBootstrapper.Instance.Resolve<IPathService>();
        }

        public List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();
            var projectEntities = projectEntityService.GetProjects();

            foreach (var item in projectEntities)
            {
                var project = ConvertToModel(item);

                GetDeadlines(project);
                GetEvents(project);
                GetTodos(project);
                GetPaths(project);

                projects.Add(project);
            }

            return projects;
        }
        public Project GetProjectByCourseId(int courseId)
        {
            var projectEntity = projectEntityService.GetProjectByCourseId(courseId);
            var project = ConvertToModel(projectEntity);

            GetDeadlines(project);
            GetEvents(project);
            GetTodos(project);
            GetPaths(project);

            return project;
        }
        private void GetDeadlines(Project project)
        {
            var deadlines = deadlineService.GetDeadlinesByProjectId(project.Id);
            project.AddDeadlineRange(deadlines);
        }
        private void GetEvents(Project project)
        {
            var events = eventService.GetEventsByProjectId(project.Id);
            project.AddEventRange(events);
        }
        private void GetTodos(Project project)
        {
            var todos = todoService.GetTodosByProjectId(project.Id);
            project.AddTodoRange(todos);
        }
        private void GetPaths(Project project)
        {
            var webpageLinks = pathService.GetWebpageLinksByProjectId(project.Id);
            project.AddWebpageLinkRange(webpageLinks);

            var filePaths = pathService.GetFilePathsByProjectId(project.Id);
            project.AddFilePathRange(filePaths);

            var folderPaths = pathService.GetFolderPathsByProjectId(project.Id);
            project.AddFolderPathRange(folderPaths);

            var applicaiontPaths = pathService.GetApplicationPathsByProjectId(project.Id);
            project.AddApplicationPathRange(applicaiontPaths);
        }

        public Project CreateProject(string title, int? courseId = null)
        {
            var project = new Project(title);

            var projectEntity = ConvertToEntity(project);
            projectEntity.CourseID = courseId;

            project.Id = projectEntityService.AddProject(projectEntity);

            return project;
        }

        public void UpdateProject(Project projectToUpdate)
        {
            var projectEntity = ConvertToEntity(projectToUpdate);

            projectEntityService.UpdateProject(projectEntity);
        }
        public async Task UpdateProjectAsync(Project projectToUpdate)
        {
            await Task.Run(() => UpdateProject(projectToUpdate));
        }

        public void DeleteProject(Project projectToDelete)
        {
            todoService.DeleteTodosByProjectId(projectToDelete.Id);

            var projectEntity = ConvertToEntity(projectToDelete);
            projectEntityService.DeleteProject(projectEntity);
        }
        public async Task DeleteProjectAsync(Project projectToDelete)
        {
            await Task.Run(() => DeleteProject(projectToDelete));
        }


        public Project ConvertToModel(ProjectEntity entity)
        {
            return new Project(entity.Title)
            {
                Id = entity.Id
            };
        }
        public ProjectEntity ConvertToEntity(Project model)
        {
            return new ProjectEntity()
            {
                Id = model.Id,
                Title = model.Title
            };
        }
    }
}
