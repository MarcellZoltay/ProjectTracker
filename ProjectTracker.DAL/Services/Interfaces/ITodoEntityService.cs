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
        List<TodoEntity> GetTodosByProjectId(int projectId);
        int AddTodo(TodoEntity todo);
        void UpdateTodo(TodoEntity todoToUpdate);
        void DeleteTodo(TodoEntity todoToDelete);
        void DeleteTodosByProjectId(int projectId);
    }
}
