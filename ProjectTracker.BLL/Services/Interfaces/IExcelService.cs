using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface IExcelService
    {
        List<List<string>> ReadExcelFile(string path);
    }
}
