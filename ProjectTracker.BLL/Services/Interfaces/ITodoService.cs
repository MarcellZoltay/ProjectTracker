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
        List<Todo> GetTodos(int id);
        Todo CreateTodo(int? projectId, int? parentTodoId, string text, DateTime? deadline);
        void UpdateTodo(Todo todo);
        Task UpdateTodoAsync(Todo todo);
        void DeleteTodo(Todo todo);
        Task DeleteTodoAsync(Todo todo);
        void DeleteTodosByProjectId(int projectId);
    }
}
