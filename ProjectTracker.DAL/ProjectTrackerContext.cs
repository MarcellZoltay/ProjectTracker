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
        public DbSet<DeadlineEntity> Deadlines { get; set; }
        public DbSet<EventEntity> Events { get; set; }
        public DbSet<TodoEntity> Todos { get; set; }
        public DbSet<PathEntity> Paths { get; set; }
        public DbSet<PathType> PathTypes { get; set; }
        public DbSet<TermEntity> Terms { get; set; }
        public DbSet<CourseEntity> Courses { get; set; }

        public ProjectTrackerContext() : base("ProjectTrackerDB")
        {
            Database.SetInitializer(new ProjectTrackerDBInitializer());
        }
    }
}
