using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using StatisticMaker.BLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.BLL.Services.Implementations
{
    public class TodoService : ITodoService, IModelEntityMapper<Todo, TodoEntity>
    {
        private ITodoEntityService todoEntityService;

        public TodoService()
        {
            todoEntityService = UnityBootstrapper.Instance.Resolve<ITodoEntityService>();
        }

        public List<Todo> GetTodos(int projectId)
        {
            List<Todo> todos = new List<Todo>();
            var todoEntities = todoEntityService.GetProjects(projectId);

            foreach (var item in todoEntities)
            {
                var todo = ConvertToModel(item);

                LoadChlidrenTodos(todo, item);

                todos.Add(todo);
            }

            return todos;
        }
        private void LoadChlidrenTodos(Todo todo, TodoEntity todoEntity)
        {
            foreach (var item in todoEntity.Children)
            {
                var todoModel = ConvertToModel(item);

                LoadChlidrenTodos(todoModel, item);

                todo.AddTodo(todoModel);
            }
        }

        public Todo CreateTodo(int? projectId, int? parentTodoId, string text, DateTime? deadline)
        {
            var todo = new Todo(text) { Deadline = deadline };

            var todoEntity = ConvertToEntity(todo);
            todoEntity.ProjectID = projectId;
            todoEntity.ParentTodoID = parentTodoId;

            todo.Id = todoEntityService.AddTodo(todoEntity);

            return todo;
        }

        public void UpdateTodo(Todo todo)
        {
            var todoEntity = ConvertToEntity(todo);

            todoEntityService.UpdateTodo(todoEntity);
        }
        public async Task UpdateTodoAsync(Todo todo)
        {
            await Task.Run(() => UpdateTodo(todo));
        }

        public void DeleteTodo(Todo todo)
        {
            var todoEntity = ConvertToEntity(todo);

            todoEntityService.DeleteTodo(todoEntity);
        }
        public async Task DeleteTodoAsync(Todo todo)
        {
            await Task.Run(() => DeleteTodo(todo));
        }

        public void DeleteTodosByProjectId(int projectId)
        {
            todoEntityService.DeleteTodosByProjectId(projectId);
        }


        public Todo ConvertToModel(TodoEntity entity)
        {
            return new Todo(entity.Text)
            {
                Id = entity.Id,
                IsInProgress = entity.IsInProgress,
                IsDone = entity.IsDone,
                Deadline = entity.Deadline
            };
        }
        public TodoEntity ConvertToEntity(Todo model)
        {
            return new TodoEntity
            {
                Id = model.Id,
                Text = model.Text,
                IsInProgress = model.IsInProgress,
                IsDone = model.IsDone,
                Deadline = model.Deadline
            };
        }
    }
}
