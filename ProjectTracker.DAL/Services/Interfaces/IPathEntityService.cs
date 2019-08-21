using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Interfaces
{
    public interface IPathEntityService
    {
        List<PathEntity> GetPathsByProjectId(int projectId);
        int AddWebpageLinkPath(PathEntity webPageLinkPath);
        int AddFilePath(PathEntity filePath);
        int AddFolderPath(PathEntity folderPath);
        int AddApplicationPath(PathEntity applicationPath);
        void DeletePath(PathEntity pathToDelete);
        void DeletePathsByProjectId(int projectId);
    }
}
