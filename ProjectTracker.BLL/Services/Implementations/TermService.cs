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
    public class TermService : ITermService, IModelEntityMapper<Term, TermEntity>
    {
        private ITermEntityService termEntityService;
        private ICourseService courseService;

        public TermService()
        {
            termEntityService = UnityBootstrapper.Instance.Resolve<ITermEntityService>();
            courseService = UnityBootstrapper.Instance.Resolve<ICourseService>();
        }

        public List<Term> GetTerms()
        {
            List<Term> terms = new List<Term>();
            var termEntities = termEntityService.GetTerms();

            foreach (var item in termEntities)
            {
                var term = ConvertToModel(item);

                var courses = courseService.GetCoursesByTermId(term.Id);
                term.AddCourseRange(courses);

                terms.Add(term);
            }

            return terms;
        }

        public Term ConvertToModel(TermEntity entity)
        {
            return new Term(entity.Title)
            {
                Id = entity.Id
            };
        }

        public TermEntity ConvertToEntity(Term model)
        {
            return new TermEntity()
            {
                Id = model.Id,
                Title = model.Title
            };
        }
    }
}
