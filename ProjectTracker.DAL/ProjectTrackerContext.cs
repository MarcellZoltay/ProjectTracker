using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL
{
    public class ProjectTrackerContext : DbContext
    {
        public DbSet<ProjectEntity> Projects { get; set; }

        public ProjectTrackerContext() : base ("ProjectTrackerDB") { }

    }
}
