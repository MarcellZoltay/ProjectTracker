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
    public class DeadlineEntityService : IDeadlineEntityService
    {
        public IEnumerable<DeadlineEntity> GetDeadlinesByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                return (from d in db.Deadlines.AsNoTracking()
                        where d.ProjectID == projectId
                        select d).ToList();
            }
        }

        public int AddDeadline(DeadlineEntity deadline)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Deadlines.Add(deadline);
                db.SaveChanges();
            }

            return deadline.Id;
        }

        public void UpdateDeadline(DeadlineEntity deadlineToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                var entity = (from d in db.Deadlines
                              where d.Id == deadlineToUpdate.Id
                              select d).First();

                deadlineToUpdate.ProjectID = entity.ProjectID;

                db.Entry(entity).State = EntityState.Detached;
                db.Entry(deadlineToUpdate).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void DeleteDeadline(DeadlineEntity deadlineToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Entry(deadlineToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
