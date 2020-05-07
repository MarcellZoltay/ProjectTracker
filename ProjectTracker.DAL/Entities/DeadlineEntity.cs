using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Deadline")]
    public class DeadlineEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime Time { get; set; }
        public bool IsCompleted { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public virtual ProjectEntity Project { get; set; }
    }
}
