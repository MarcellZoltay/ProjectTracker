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
        Project GetProjectByCourseId(int courseId);
        Project CreateProject(string projectTitle, int? courseId = null);
        void UpdateProject(Project projectToUpdate);
        Task UpdateProjectAsync(Project projectToUpdate);
        void DeleteProject(Project projectToDelete);
        Task DeleteProjectAsync(Project projectToDelete);
    }
}
