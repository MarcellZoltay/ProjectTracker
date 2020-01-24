using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Implementations
{
    public class TermEntityService : ITermEntityService
    {
        public List<TermEntity> GetTerms()
        {
            using (var db = new ProjectTrackerContext())
            {
                return db.Terms.AsNoTracking().ToList();
            }
        }
    }
}
