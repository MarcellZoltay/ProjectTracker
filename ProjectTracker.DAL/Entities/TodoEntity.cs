using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Todo")]
    public class TodoEntity
    {
        public TodoEntity()
        {
            this.Children = new List<TodoEntity>();
        }

        public int Id { get; set; }
        public string Text { get; set; }
        public bool IsInProgress { get; set; }
        public bool IsDone { get; set; }
        public DateTime? Deadline { get; set; }

        [ForeignKey("Project")]
        public int? ProjectID { get; set; }
        public virtual ProjectEntity Project { get; set; }

        [ForeignKey("ParentTodo")]
        public int? ParentTodoID { get; set; }
        public virtual TodoEntity ParentTodo { get; set; }

        public virtual List<TodoEntity> Children { get; set; }
    }
}
