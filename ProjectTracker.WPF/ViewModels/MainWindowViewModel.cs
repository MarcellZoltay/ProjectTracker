using System;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.WPF.Views;
using Unity;

namespace ProjectTracker.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title => "Project Tracker";

        private readonly IRegionManager regionManager;
        private readonly IUnityContainer unityContainer;

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            this.regionManager = regionManager;
            this.unityContainer = unityContainer;

            regionManager.RegisterViewWithRegion("MainRegion", () => unityContainer.Resolve<StartPage>());
        }
    }
}
