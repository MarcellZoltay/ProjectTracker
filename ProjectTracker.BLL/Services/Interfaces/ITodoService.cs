using ProjectTracker.BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Interfaces
{
    public interface ITodoService
    {
        List<Todo> GetTodosByProjectId(int projectId);
        void AddTodoToProject(int projectId, Todo todo);
        void AddSubTodo(int parentTodoId, Todo todo);
        void UpdateTodo(Todo todoToUpdate);
        Task UpdateTodoAsync(Todo todoToUpdate);
        void DeleteTodo(Todo todoToDelete);
        Task DeleteTodoAsync(Todo todoToDelete);
        void DeleteTodosByProjectId(int projectId);
    }
}
