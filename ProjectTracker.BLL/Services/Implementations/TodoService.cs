using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
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

        public List<Todo> GetTodosByProjectId(int projectId)
        {
            List<Todo> todos = new List<Todo>();
            var todoEntities = todoEntityService.GetTodosByProjectId(projectId);

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

        public void AddTodoToProject(int projectId, Todo todo)
        {
            var todoEntity = ConvertToEntity(todo);
            todoEntity.ProjectID = projectId;

            todo.Id = todoEntityService.AddTodo(todoEntity);
        }

        public void AddSubTodo(int parentTodoId, Todo todo)
        {
            var todoEntity = ConvertToEntity(todo);
            todoEntity.ParentTodoID = parentTodoId;

            todo.Id = todoEntityService.AddTodo(todoEntity);
        }

        public void UpdateTodo(Todo todoToUpdate)
        {
            var todoEntity = ConvertToEntity(todoToUpdate);

            todoEntityService.UpdateTodo(todoEntity);
        }
        public async Task UpdateTodoAsync(Todo todoToUpdate)
        {
            await Task.Run(() => UpdateTodo(todoToUpdate));
        }

        public void DeleteTodo(Todo todoToDelete)
        {
            var todoEntity = ConvertToEntity(todoToDelete);

            todoEntityService.DeleteTodo(todoEntity);
        }
        public async Task DeleteTodoAsync(Todo todoToDelete)
        {
            await Task.Run(() => DeleteTodo(todoToDelete));
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
            return new TodoEntity()
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
