using ProjectTracker.BLL.Services.Implementations;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.DAL.Services.Implementations;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace ProjectTracker.BLL
{
    public class UnityBootstrapper
    {
        #region Singleton
        private static UnityBootstrapper unityBootstrapper = new UnityBootstrapper();
        public static UnityBootstrapper Instance
        {
            get { return unityBootstrapper; }
        }

        private UnityBootstrapper()
        {
            container = new UnityContainer();

            ConfigureDependencies();
        }
        #endregion

        private IUnityContainer container;

        private void ConfigureDependencies()
        {
            container.RegisterType<IProjectEntityService, ProjectEntityService>();
            container.RegisterType<IDeadlineEntityService, DeadlineEntityService>();
            container.RegisterType<IEventEntityService, EventEntityService>();
            container.RegisterType<ITodoEntityService, TodoEntityService>();
            container.RegisterType<IPathEntityService, PathEntityService>();
            container.RegisterType<ITermEntityService, TermEntityService>();
            container.RegisterType<ICourseEntityService, CourseEntityService>();

            container.RegisterType<IProjectService, ProjectService>();
            container.RegisterType<IDeadlineService, DeadlineService>();
            container.RegisterType<IEventService, EventService>();
            container.RegisterType<ITodoService, TodoService>();
            container.RegisterType<IPathService, PathService>();
            container.RegisterType<ICourseService, CourseService>();
            container.RegisterType<IExcelService, ExcelService>();
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
