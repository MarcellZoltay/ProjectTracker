using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface IPathService
    {
        List<Path> GetPathsByProjectId(int projectId);
        Path CreateWebpageLinkPath(int projectId, string address);
        Path CreateFilePath(int projectId, string address);
        Path CreateFolderPath(int projectId, string address);
        Path CreateApplicationPath(int projectId, string address);
        void DeletePath(Path pathToDelete);
        Task DeletePathAsync(Path pathToDelete);
        void DeletePathsByProjectId(int projectId);
        void OpenPath(Path path);
        void OpenWebpageLink(string browserPath, Path path);
    }
}
