using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Entities
{
    [Table("Path")]
    public class PathEntity
    {
        public int Id { get; set; }
        public string Address { get; set; }

        [ForeignKey("PathType")]
        public int PathTypeID { get; set; }
        public virtual PathType PathType { get; set; }

        [ForeignKey("Project")]
        public int ProjectID { get; set; }
        public virtual ProjectEntity Project { get; set; }
    }
}
