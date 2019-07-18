using ProjectTracker.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Interfaces
{
    public interface ITodoEntityService
    {
        List<TodoEntity> GetProjects(int projectId);
        int AddTodo(TodoEntity todoEntity);
        void UpdateTodo(TodoEntity todoEntityToUpdate);
        void DeleteTodo(TodoEntity todoEntityToDelete);
        void DeleteTodosByProjectId(int projectId);
    }
}
