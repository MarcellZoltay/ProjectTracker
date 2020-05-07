using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Implementations
{
    public class DeadlineService : IDeadlineService, IModelEntityMapper<Deadline, DeadlineEntity>
    {
        private IDeadlineEntityService deadlineEntityService;

        public DeadlineService()
        {
            deadlineEntityService = UnityBootstrapper.Instance.Resolve<IDeadlineEntityService>();
        }

        public List<Deadline> GetDeadlinesByProjectId(int projectId)
        {
            List<Deadline> deadlines = new List<Deadline>();
            var deadlineEntities = deadlineEntityService.GetDeadlinesByProjectId(projectId);

            foreach (var item in deadlineEntities)
            {
                var deadline = ConvertToModel(item);
                deadlines.Add(deadline);
            }

            return deadlines;
        }

        public void AddDeadlineToProject(int projectId, Deadline deadline)
        {
            var deadlineEntity = ConvertToEntity(deadline);
            deadlineEntity.ProjectID = projectId;

            deadline.Id = deadlineEntityService.AddDeadline(deadlineEntity);
        }

        public void UpdateDeadline(Deadline deadlineToUpdate)
        {
            var deadlineEntity = ConvertToEntity(deadlineToUpdate);

            deadlineEntityService.UpdateDeadline(deadlineEntity);
        }

        public async Task UpdateDeadlineAsync(Deadline deadlineToUpdate)
        {
            await Task.Run(() => UpdateDeadline(deadlineToUpdate));
        }

        public void DeleteDeadline(Deadline deadlineToDelete)
        {
            var deadlineEntity = ConvertToEntity(deadlineToDelete);

            deadlineEntityService.DeleteDeadline(deadlineEntity);
        }

        public async Task DeleteDeadlineAsync(Deadline deadlineToDelete)
        {
            await Task.Run(() => DeleteDeadline(deadlineToDelete));
        }

        public Deadline ConvertToModel(DeadlineEntity entity)
        {
            return new Deadline(entity.Text, entity.Time)
            {
                Id = entity.Id,
                IsCompleted = entity.IsCompleted
            };
        }

        public DeadlineEntity ConvertToEntity(Deadline model)
        {
            return new DeadlineEntity()
            {
                Id = model.Id,
                Text = model.Text,
                Time = model.Time,
                IsCompleted = model.IsCompleted
            };
        }
    }
}
