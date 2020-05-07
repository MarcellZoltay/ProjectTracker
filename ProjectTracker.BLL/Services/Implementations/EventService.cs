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
    public class EventService : IEventService, IModelEntityMapper<Event, EventEntity>
    {
        private IEventEntityService eventEntityService;

        public EventService()
        {
            eventEntityService = UnityBootstrapper.Instance.Resolve<IEventEntityService>();
        }

        public List<Event> GetEventsByProjectId(int projectId)
        {
            List<Event> events = new List<Event>();
            var eventEntities = eventEntityService.GetEventsByProjectId(projectId);

            foreach (var item in eventEntities)
            {
                var @event = ConvertToModel(item);
                events.Add(@event);
            }

            return events;
        }

        public void AddEventToProject(int projectId, Event @event)
        {
            var eventEntity = ConvertToEntity(@event);
            eventEntity.ProjectID = projectId;

            @event.Id = eventEntityService.AddEvent(eventEntity);
        }

        public void UpdateEvent(Event eventToUpdate)
        {
            var eventEntity = ConvertToEntity(eventToUpdate);

            eventEntityService.UpdateEvent(eventEntity);
        }

        public async Task UpdateEventAsync(Event eventToUpdate)
        {
            await Task.Run(() => UpdateEvent(eventToUpdate));
        }

        public void DeleteEvent(Event eventToDelete)
        {
            var eventEntity = ConvertToEntity(eventToDelete);

            eventEntityService.DeleteEvent(eventEntity);
        }

        public async Task DeleteEventAsync(Event eventToDelete)
        {
            await Task.Run(() => DeleteEvent(eventToDelete));
        }

        public Event ConvertToModel(EventEntity entity)
        {
            return new Event(entity.Text, entity.StartTime, entity.EndTime)
            {
                Id = entity.Id,
                WasPresent = entity.WasPresent
            };
        }

        public EventEntity ConvertToEntity(Event model)
        {
            return new EventEntity()
            {
                Id = model.Id,
                Text = model.Text,
                StartTime = model.StartTime,
                EndTime = model.EndTime,
                WasPresent = model.WasPresent
            };
        }
    }
}
