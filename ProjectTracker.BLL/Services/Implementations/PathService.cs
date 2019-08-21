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

        public List<Path> GetPathsByProjectId(int projectId)
        {
            List<Path> paths = new List<Path>();
            var pathEntities = pathEntityService.GetPathsByProjectId(projectId);

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
            path.Type = pathEntity.PathType.Name;

            return path;
        }
        public Path CreateFilePath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddFilePath(pathEntity);
            path.Type = pathEntity.PathType.Name;

            return path;
        }
        public Path CreateFolderPath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddFolderPath(pathEntity);
            path.Type = pathEntity.PathType.Name;

            return path;
        }
        public Path CreateApplicationPath(int projectId, string address)
        {
            var path = new Path(address);

            var pathEntity = ConvertToEntity(path);
            pathEntity.ProjectID = projectId;

            path.Id = pathEntityService.AddApplicationPath(pathEntity);
            path.Type = pathEntity.PathType.Name;

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
        public void OpenWebpageLink(string browserPath, Path path)
        {
            Process.Start(browserPath, path.Address);
        }

        public Path ConvertToModel(PathEntity entity)
        {
            return new Path(entity.Address)
            {
                Id = entity.Id,
                Type = entity.PathType.Name
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
