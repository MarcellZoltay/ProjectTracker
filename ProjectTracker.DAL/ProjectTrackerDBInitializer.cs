using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL
{
    public class ProjectTrackerDBInitializer : CreateDatabaseIfNotExists<ProjectTrackerContext>
    {
        protected override void Seed(ProjectTrackerContext context)
        {
            IList<PathType> pathTypes = new List<PathType>();

            pathTypes.Add(new PathType() { Name = "Webpage link" });
            pathTypes.Add(new PathType() { Name = "File" });
            pathTypes.Add(new PathType() { Name = "Folder" });
            pathTypes.Add(new PathType() { Name = "Application" });

            context.PathTypes.AddRange(pathTypes);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
