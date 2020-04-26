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
    public class EventEntityService : IEventEntityService
    {
        public IEnumerable<EventEntity> GetEventsByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from e in db.Events.AsNoTracking()
                        where e.ProjectID == projectId
                        select e).ToList();
            }
        }

        public int AddEvent(EventEntity @event)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Events.Add(@event);
                db.SaveChanges();
            }

            return @event.Id;
        }

        public void UpdateEvent(EventEntity eventToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                var entity = (from e in db.Events
                              where e.Id == eventToUpdate.Id
                              select e).First();

                eventToUpdate.ProjectID = entity.ProjectID;

                db.Entry(entity).State = EntityState.Detached;
                db.Entry(eventToUpdate).State = EntityState.Modified;

                db.SaveChanges();
            }
        }
        public void DeleteEvent(EventEntity eventToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(eventToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
