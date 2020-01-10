using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Models
{
    public class Path : Bindable
    {
        public int Id { get; set; }

        private string address;
        public string Address
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        public Path(string address)
        {
            Address = address;
        }
    }
}
