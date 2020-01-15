using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Term")]
    public class TermEntity
    {
        public TermEntity()
        {
            this.Courses = new List<CourseEntity>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual List<CourseEntity> Courses { get; set; }
    }
}
