using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Course")]
    public class CourseEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Credit { get; set; }
        public bool IsFulfilled { get; set; }

        [ForeignKey(nameof(Term))]
        public int TermID { get; set; }
        public virtual TermEntity Term { get; set; }
    }
}
