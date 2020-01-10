using Microsoft.Win32;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Implementations;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Implementations
{
    public class PathService : IPathService, IModelEntityMapper<Path, PathEntity>
    {
        private IPathEntityService pathEntityService;

        public PathService()
        {
            pathEntityService = UnityBootstrapper.Instance.Resolve<IPathEntityService>();
        }

        public List<Path> GetWebpageLinksByProjectId(int projectId)
        {
            var pathEntities = pathEntityService.GetWebpageLinksByProjectId(projectId);
            var paths = ConvertToPaths(pathEntities);

            return paths;
        }
        public List<Path> GetFilePathsByProjectId(int projectId)
        {
            var pathEntities = pathEntityService.GetFilePathsByProjectId(projectId);
            var paths = ConvertToPaths(pathEntities);

            return paths;
        }
        public List<Path> GetFolderPathsByProjectId(int projectId)
        {
            var pathEntities = pathEntityService.GetFolderPathsByProjectId(projectId);
            var paths = ConvertToPaths(pathEntities);

            return paths;
        }
        public List<Path> GetApplicationPathsByProjectId(int projectId)
        {
            var pathEntities = pathEntityService.GetApplicationPathsByProjectId(projectId);
            var paths = ConvertToPaths(pathEntities);

            return paths;
        }

        private List<Path> ConvertToPaths(List<PathEntity> pathEntities)
        {
            List<Path> paths = new List<Path>();

            foreach (var item in pathEntities)
            {
                var path = ConvertToModel(item);
                paths.Add(path);
            }
        
            return paths;
        }

        public Path CreateWebpageLinkPath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddWebpageLinkPath(pathEntity);

            return path;
        }
        public Path CreateFilePath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddFilePath(pathEntity);

            return path;
        }
        public Path CreateFolderPath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddFolderPath(pathEntity);

            return path;
        }
        public Path CreateApplicationPath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddApplicationPath(pathEntity);

            return path;
        }

        public void DeletePath(Path pathToDelete)
        {
            var pathEntity = ConvertToEntity(pathToDelete);

            pathEntityService.DeletePath(pathEntity);
        }
        public async Task DeletePathAsync(Path pathToDelete)
        {
            await Task.Run(() => DeletePath(pathToDelete));
        }

        public void DeletePathsByProjectId(int projectId)
        {
            pathEntityService.DeletePathsByProjectId(projectId);
        }

        public void OpenPath(Path path)
        {
            Process.Start(path.Address);
        }
        public void OpenWebpageLink(Path path)
        {
            Process.Start(GetDefaultBrowserPath(), path.Address);
        }

        public string GetDefaultBrowserPath()
        {
            string urlAssociation = @"Software\Microsoft\Windows\Shell\Associations\UrlAssociations\http";
            string browserPathKey = @"$BROWSER$\shell\open\command";

            RegistryKey userChoiceKey = null;
            string browserPath = "";

            try
            {
                //Read default browser path from userChoiceLKey
                userChoiceKey = Registry.CurrentUser.OpenSubKey(urlAssociation + @"\UserChoice", false);

                //If user choice was not found, try machine default
                if (userChoiceKey == null)
                {
                    //Read default browser path from Win XP registry key
                    var browserKey = Registry.ClassesRoot.OpenSubKey(@"HTTP\shell\open\command", false);

                    //If browser path wasn’t found, try Win Vista (and newer) registry key
                    if (browserKey == null)
                    {
                        browserKey = Registry.CurrentUser.OpenSubKey(urlAssociation, false);
                    }
                    var path = ClarifyBrowserPath(browserKey.GetValue(null) as string);
                    browserKey.Close();
                    return path;
                }
                else
                {
                    // user defined browser choice was found
                    string progId = (userChoiceKey.GetValue("ProgId").ToString());
                    userChoiceKey.Close();

                    // now look up the path of the executable
                    string concreteBrowserKey = browserPathKey.Replace("$BROWSER$", progId);
                    var kp = Registry.ClassesRoot.OpenSubKey(concreteBrowserKey, false);
                    browserPath = ClarifyBrowserPath(kp.GetValue(null) as string);
                    kp.Close();
                    return browserPath;
                }
            }
            catch
            {
                return "";
            }
        }
        private string ClarifyBrowserPath(string path)
        {
            var parts = path.Split('\"');

            foreach (var p in parts)
            {
                if (p.Contains(".exe"))
                    return p;

            }

            return "";
        }

        public Path ConvertToModel(PathEntity entity)
        {
            return new Path(entity.Address)
            {
                Id = entity.Id
            };
        }
        public PathEntity ConvertToEntity(Path model)
        {
            return new PathEntity
            {
                Id = model.Id,
                Address = model.Address
            };
        }
    }
}
