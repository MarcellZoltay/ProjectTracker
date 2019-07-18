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
        public List<TodoEntity> GetProjects(int projectId)
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
        private void LoadChlidrenTodoEntities(ProjectTrackerContext db, TodoEntity todoEntity)
        {
            var todoEntities = (from t in db.Todos.Include("Project").Include("ParentTodo").AsNoTracking()
                                where t.ParentTodoID == todoEntity.Id
                                select t).ToList();

            todoEntity.Children.Clear();
            todoEntity.Children.AddRange(todoEntities);

            foreach (var item in todoEntities)
            {
                LoadChlidrenTodoEntities(db, item);
            }
        }

        public int AddTodo(TodoEntity todoEntity)
        {
            using (var db = new ProjectTrackerContext())
            {
                db.Todos.Add(todoEntity);
                db.SaveChanges();
            }

            return todoEntity.Id;
        }

        public void UpdateTodo(TodoEntity todoEntityToUpdate)
        {
            using (var db = new ProjectTrackerContext())
            {
                var entity = (from t in db.Todos
                             where t.Id == todoEntityToUpdate.Id
                             select t).First();

                todoEntityToUpdate.ProjectID = entity.ProjectID;
                todoEntityToUpdate.ParentTodoID = entity.ParentTodoID;

                db.Entry(entity).State = EntityState.Detached;
                db.Entry(todoEntityToUpdate).State = EntityState.Modified;

                db.SaveChanges();
            }
        }

        public void DeleteTodo(TodoEntity todoEntityToDelete)
        {
            using (var db = new ProjectTrackerContext())
            {
                DeleteChlidrenTodoEntities(db, todoEntityToDelete);

                db.Entry(todoEntityToDelete).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
        private void DeleteChlidrenTodoEntities(ProjectTrackerContext db, TodoEntity todoEntity)
        {
            var todoEntities = (from t in db.Todos
                                where t.ParentTodoID == todoEntity.Id
                                select t).ToList();

            foreach (var item in todoEntities)
            {
                DeleteChlidrenTodoEntities(db, item);

                db.Entry(item).State = EntityState.Deleted;
            }
        }

        public void DeleteTodosByProjectId(int projectId)
        {
            using (var db = new ProjectTrackerContext())
            {
                var todoEntities = (from t in db.Todos
                                    where t.ProjectID == projectId
                                    select t).ToList();

                foreach (var item in todoEntities)
                {
                    DeleteChlidrenTodoEntities(db, item);

                    db.Entry(item).State = EntityState.Deleted;
                }

                db.SaveChanges();
            }
        }
    }
}
