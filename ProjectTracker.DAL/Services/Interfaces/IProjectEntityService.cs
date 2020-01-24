using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Interfaces
{
    public interface IProjectEntityService
    {
        List<ProjectEntity> GetProjects();
        ProjectEntity GetProjectByCourseId(int courseId);
        int AddProject(ProjectEntity project);
        void UpdateProject(ProjectEntity projectToUpdate);
        void DeleteProject(ProjectEntity projectToDelete);
    }
}
