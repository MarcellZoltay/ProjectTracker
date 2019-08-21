using ProjectTracker.DAL.Entities;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.DAL.Services.Implementations
{
    public class TodoEntityService : ITodoEntityService
    {
        public List<TodoEntity> GetTodosByProjectId(int projectId)
        {
            var todos = new List<TodoEntity>();

            using (var db = new ProjectTrackerContext())
            {
                var query = (from t in db.Todos.Include("Project").Include("ParentTodo").AsNoTracking()
                             where t.ProjectID == projectId
                             select t).ToList();

                todos.AddRange(query);

                foreach (var item in query)
                {
                    LoadChlidrenTodoEntities(db, item);
                }
            }

            return todos;
        }
        private void LoadChlidrenTodoEntities(ProjectTrackerContext db, TodoEntity todo)
        {
            var todos = (from t in db.Todos.Include("Project").Include("ParentTodo").AsNoTracking()
                         where t.ParentTodoID == todo.Id
                         select t).ToList();

            todo.Children.Clear();
            todo.Children.AddRange(todos);

            foreach (var item in todos)
            {
                LoadChlidrenTodoEntities(db, item);
            }
        }

        public int AddTodo(TodoEntity todo)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Todos.Add(todo);
                db.SaveChanges();
            }

            return todo.Id;
        }

        public void UpdateTodo(TodoEntity todoToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                var entity = (from t in db.Todos
                              where t.Id == todoToUpdate.Id
                              select t).First();

                todoToUpdate.ProjectID = entity.ProjectID;
                todoToUpdate.ParentTodoID = entity.ParentTodoID;

                db.Entry(entity).State = EntityState.Detached;
                db.Entry(todoToUpdate).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void DeleteTodo(TodoEntity todoToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                DeleteChlidrenTodoEntities(db, todoToDelete);

                db.Entry(todoToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        private void DeleteChlidrenTodoEntities(ProjectTrackerContext db, TodoEntity todo)
        {
            var todos = (from t in db.Todos
                         where t.ParentTodoID == todo.Id
                         select t).ToList();

            foreach (var item in todos)
            {
                DeleteChlidrenTodoEntities(db, item);

                db.Entry(item).State = EntityState.Deleted;
            }
        }

        public void DeleteTodosByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                var todos = (from t in db.Todos
                             where t.ProjectID == projectId
                             select t).ToList();

                foreach (var item in todos)
                {
                    DeleteChlidrenTodoEntities(db, item);

                    db.Entry(item).State = EntityState.Deleted;
                }

                db.SaveChanges();
            }
        }
    }
}
