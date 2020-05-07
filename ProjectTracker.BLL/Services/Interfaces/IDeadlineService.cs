using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface IDeadlineService
    {
        List<Deadline> GetDeadlinesByProjectId(int projectId);
        void AddDeadlineToProject(int projectId, Deadline deadline);
        void UpdateDeadline(Deadline deadlineToUpdate);
        Task UpdateDeadlineAsync(Deadline deadlineToUpdate);
        void DeleteDeadline(Deadline deadlineToDelete);
        Task DeleteDeadlineAsync(Deadline deadlineToDelete);
    }
}
