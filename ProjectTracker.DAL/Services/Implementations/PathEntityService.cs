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
    public class PathEntityService : IPathEntityService
    {
        private const string WebPageLink = "Webpage link";
        private const string File = "File";
        private const string Folder = "Folder";
        private const string Application = "Application";

        public List<PathEntity> GetWebpageLinksByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from p in db.Paths.Include("PathType").AsNoTracking()
                        where p.ProjectID == projectId && p.PathType.Name == WebPageLink
                        select p).ToList();
            }
        }
        public List<PathEntity> GetFilePathsByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from p in db.Paths.Include("PathType").AsNoTracking()
                        where p.ProjectID == projectId && p.PathType.Name == File
                        select p).ToList();
            }
        }
        public List<PathEntity> GetFolderPathsByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from p in db.Paths.Include("PathType").AsNoTracking()
                        where p.ProjectID == projectId && p.PathType.Name == Folder
                        select p).ToList();
            }
        }
        public List<PathEntity> GetApplicationPathsByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from p in db.Paths.Include("PathType").AsNoTracking()
                        where p.ProjectID == projectId && p.PathType.Name == Application
                        select p).ToList();
            }
        }

        public int AddWebpageLinkPath(PathEntity webPageLinkPath)
        {
            using (var db = new ProjectTrackerContext())
            {
                var type = from t in db.PathTypes
                           where t.Name == WebPageLink
                           select t;

                webPageLinkPath.PathType = type.First();

                db.Paths.Add(webPageLinkPath);
                db.SaveChanges();
            }

            return webPageLinkPath.Id;
        }
        public int AddFilePath(PathEntity filePath)
        {
            using (var db = new ProjectTrackerContext())
            {
                var type = from t in db.PathTypes
                           where t.Name == File
                           select t;

                filePath.PathType = type.First();

                db.Paths.Add(filePath);
                db.SaveChanges();
            }

            return filePath.Id;
        }
        public int AddFolderPath(PathEntity folderPath)
        {
            using (var db = new ProjectTrackerContext())
            {
                var type = from t in db.PathTypes
                           where t.Name == Folder
                           select t;

                folderPath.PathType = type.First();

                db.Paths.Add(folderPath);
                db.SaveChanges();
            }

            return folderPath.Id;
        }
        public int AddApplicationPath(PathEntity applicationPath)
        {
            using (var db = new ProjectTrackerContext())
            {
                var type = from t in db.PathTypes
                           where t.Name == Application
                           select t;

                applicationPath.PathType = type.First();

                db.Paths.Add(applicationPath);
                db.SaveChanges();
            }

            return applicationPath.Id;
        }

        public void DeletePath(PathEntity pathToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(pathToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        public void DeletePathsByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                var paths = (from p in db.Paths
                             where p.ProjectID == projectId
                             select p).ToList();

                db.Paths.RemoveRange(paths);

                db.SaveChanges();
            }
        }
    }
}
