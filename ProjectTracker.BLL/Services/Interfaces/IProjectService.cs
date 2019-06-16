using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface IProjectService
    {
        List<Project> GetProjects();
        Project CreateProject(string projectTitle);
        void UpdateProject(Project project);
        Task UpdateProjectAsync(Project project);
        void DeleteProject(Project project);
        Task DeleteProjectAsync(Project project);
    }
}
