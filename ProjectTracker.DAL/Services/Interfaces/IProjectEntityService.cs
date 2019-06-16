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
        int AddProject(ProjectEntity projectEntity);
        void UpdateProject(ProjectEntity projectEntityToUpdate);
        void DeleteProject(ProjectEntity projectEntityToDelete);
    }
}
