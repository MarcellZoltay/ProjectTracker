using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Implementations
{
    public class ProjectEntityService : IProjectEntityService
    {
        public List<ProjectEntity> GetProjects()
        {
            using (var db = new ProjectTrackerContext())
            {
                return db.Projects.AsNoTracking().ToList();
            }
        }

        public int AddProject(ProjectEntity projectEntity)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Projects.Add(projectEntity);
                db.SaveChanges();
            }

            return projectEntity.Id;
        }

        public void UpdateProject(ProjectEntity projectEntityToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(projectEntityToUpdate).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteProject(ProjectEntity projectEntityToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(projectEntityToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

    }
}
