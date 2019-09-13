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
        private ITodoService todoService;
        private IPathService pathService;

        public ProjectService()
        {
            projectEntityService = UnityBootstrapper.Instance.Resolve<IProjectEntityService>();
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

                var todos = todoService.GetTodosByProjectId(project.Id);
                project.AddTodoRange(todos);

                var paths = pathService.GetPathsByProjectId(project.Id);
                project.AddPathRange(paths);

                projects.Add(project);
            }

            return projects;
        }

        public Project CreateProject(string title)
        {
            var project = new Project(title);

            var projectEntity = ConvertToEntity(project);

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
            pathService.DeletePathsByProjectId(projectToDelete.Id);

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
