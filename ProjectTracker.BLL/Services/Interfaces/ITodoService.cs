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
        Todo CreateTodo(int? projectId, int? parentTodoId, string text, DateTime? deadline);
        void UpdateTodo(Todo todoToUpdate);
        Task UpdateTodoAsync(Todo todoToUpdate);
        void DeleteTodo(Todo todoToDelete);
        Task DeleteTodoAsync(Todo todoToDelete);
        void DeleteTodosByProjectId(int projectId);
    }
}
