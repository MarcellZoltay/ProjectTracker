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
        private IExcelService excelService;

        public TermService()
        {
            termEntityService = UnityBootstrapper.Instance.Resolve<ITermEntityService>();
            courseService = UnityBootstrapper.Instance.Resolve<ICourseService>();
            excelService = UnityBootstrapper.Instance.Resolve<IExcelService>();
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

        public void ImportLessonsAsEventsFromExcel(string path, Term term)
        {
            var rows = excelService.ReadExcelFile(path);

            var courses = term.Courses;

            for (int i = 1; i < rows.Count; i++)
            {
                var row = rows[i];

                string[] start = row.ElementAt(0).Split('.', ':');
                DateTime startTime = new DateTime(int.Parse(start[0]), int.Parse(start[1]), int.Parse(start[2]),
                                                  int.Parse(start[3].Trim()), int.Parse(start[4]), int.Parse(start[5]));

                string[] end = row.ElementAt(1).Split('.', ':');
                DateTime endTime = new DateTime(int.Parse(end[0]), int.Parse(end[1]), int.Parse(end[2]),
                                                  int.Parse(end[3].Trim()), int.Parse(end[4]), int.Parse(end[5]));

                string[] summary = row.ElementAt(2).Split('(', ')');
                string subjectTitle = summary[0].Trim();
                string lessonType = summary[2].Trim().Split('-')[1].Trim();

                string venue = row.ElementAt(3);

                var course = courses.Where(c => subjectTitle.Contains(c.Title)).FirstOrDefault();
                if (course != null)
                    courseService.ImportLessonEvent(course, startTime, endTime, lessonType, venue);
            }
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
