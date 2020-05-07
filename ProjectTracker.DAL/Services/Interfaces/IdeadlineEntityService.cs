using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Interfaces
{
    public interface IDeadlineEntityService
    {
        IEnumerable<DeadlineEntity> GetDeadlinesByProjectId(int projectId);
        int AddDeadline(DeadlineEntity deadlineEntity);
        void UpdateDeadline(DeadlineEntity deadlineEntity);
        void DeleteDeadline(DeadlineEntity deadlineEntity);
    }
}
