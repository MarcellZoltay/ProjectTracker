using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface IEventService
    {
        List<Event> GetEventsByProjectId(int projectId);
        void AddEventToProject(int projectId, Event @event);
        void UpdateEvent(Event @eventToUpdate);
        Task UpdateEventAsync(Event @eventToUpdate);
        void DeleteEvent(Event @eventToDelete);
        Task DeleteEventAsync(Event @eventToDelete);
    }
}
