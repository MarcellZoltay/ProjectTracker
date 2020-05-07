using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Event")]
    public class EventEntity
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public bool WasPresent { get; set; }

        [ForeignKey(nameof(Project))]
        public int ProjectID { get; set; }
        public virtual ProjectEntity Project { get; set; }
    }
}
