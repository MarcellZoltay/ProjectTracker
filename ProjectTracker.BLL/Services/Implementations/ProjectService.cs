using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using StatisticMaker.BLL;
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

        public ProjectService()
        {
            projectEntityService = UnityBootstrapper.Instance.Resolve<IProjectEntityService>();
        }

        public List<Project> GetProjects()
        {
            List<Project> projects = new List<Project>();
            var projectEntities = projectEntityService.GetProjects();

            foreach (var item in projectEntities)
            {
                var project = ConvertToModel(item);
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

        public void UpdateProject(Project project)
        {
            var projectEntity = ConvertToEntity(project);

            projectEntityService.UpdateProject(projectEntity);
        }
        public async Task UpdateProjectAsync(Project project)
        {
            await Task.Run(() => UpdateProject(project));
        }

        public void DeleteProject(Project project)
        {
            var projectEntity = ConvertToEntity(project);

            projectEntityService.DeleteProject(projectEntity);
        }
        public async Task DeleteProjectAsync(Project project)
        {
            await Task.Run(() => DeleteProject(project));
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
            return new ProjectEntity
            {
                Id = model.Id,
                Title = model.Title
            };
        }

    }
}
