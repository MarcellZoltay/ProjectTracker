using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Project")]
    public class ProjectEntity
    {
        public ProjectEntity()
        {
            this.Todos = new HashSet<TodoEntity>();
        }

        public int Id { get; set; }
        public string Title { get; set; }

        public virtual ICollection<TodoEntity> Todos { get; set; }
    }
}
