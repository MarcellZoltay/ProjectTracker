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
            //IList<PathType> pathTypes = new List<PathType>();

            //pathTypes.Add(new PathType() { Name = "Webpage link" });
            //pathTypes.Add(new PathType() { Name = "File" });
            //pathTypes.Add(new PathType() { Name = "Folder" });
            //pathTypes.Add(new PathType() { Name = "Application" });

            //context.PathTypes.AddRange(pathTypes);

            IList<TermEntity> terms = new List<TermEntity>();

            terms.Add(new TermEntity() { Title = "1. Term" });
            terms.Add(new TermEntity() { Title = "2. Term" });
            terms.Add(new TermEntity() { Title = "3. Term" });
            terms.Add(new TermEntity() { Title = "4. Term" });

            context.Terms.AddRange(terms);

            context.SaveChanges();

            base.Seed(context);
        }
    }
}
