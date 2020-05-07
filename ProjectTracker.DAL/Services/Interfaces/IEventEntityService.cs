using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Interfaces
{
    public interface IEventEntityService
    {
        IEnumerable<EventEntity> GetEventsByProjectId(int projectId);
        int AddEvent(EventEntity eventEntity);
        void UpdateEvent(EventEntity eventEntity);
        void DeleteEvent(EventEntity eventEntity);
    }
}
