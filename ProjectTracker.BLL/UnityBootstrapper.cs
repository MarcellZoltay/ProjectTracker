using ProjectTracker.DAL.Services.Implementations;
using ProjectTracker.DAL.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace StatisticMaker.BLL
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
        }

        public T Resolve<T>()
        {
            return container.Resolve<T>();
        }
    }
}
