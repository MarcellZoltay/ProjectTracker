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

        public int AddProject(ProjectEntity project)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Projects.Add(project);
                db.SaveChanges();
            }

            return project.Id;
        }

        public void UpdateProject(ProjectEntity projectToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(projectToUpdate).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public void DeleteProject(ProjectEntity projectToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(projectToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

    }
}
