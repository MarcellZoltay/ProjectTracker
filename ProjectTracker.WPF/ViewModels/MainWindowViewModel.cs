using System;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.WPF.Constants;
using ProjectTracker.WPF.Views;
using Unity;

namespace ProjectTracker.WPF.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        public string Title => "Project Tracker";

        public DelegateCommand GoBackClickCommand { get; }

        private readonly IRegionManager regionManager;

        private readonly IRegion mainRegion;
        private readonly StartPage startPage;

        private bool showNavigationBar;
        public bool ShowNavigationBar
        {
            get { return showNavigationBar; }
            set { SetProperty(ref showNavigationBar, value); }
        }

        private string navigationBarTitle;
        public string NavigationBarTitle
        {
            get { return navigationBarTitle; }
            set { SetProperty(ref navigationBarTitle, value); }
        }

        public MainWindowViewModel(IRegionManager regionManager, IUnityContainer unityContainer)
        {
            GoBackClickCommand = new DelegateCommand(GoBackCommand);

            this.regionManager = regionManager;

            mainRegion = regionManager.Regions[RegionNames.MainRegion];
            mainRegion.ActiveViews.CollectionChanged += ActiveViews_CollectionChanged;

            startPage = unityContainer.Resolve<StartPage>();
            mainRegion.Add(startPage, PageNames.StartPage);
            mainRegion.Activate(startPage);
            mainRegion.Context = NavigationBarTitle;

            mainRegion.NavigationService.Navigated += NavigationService_Navigated;
        }

        private void GoBackCommand()
        {
            regionManager.RequestNavigate(RegionNames.MainRegion, PageNames.StartPage);
        }

        private void ActiveViews_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            ShowNavigationBar = mainRegion.ActiveViews.Contains(startPage) ? false : true;
        }

        private void NavigationService_Navigated(object sender, RegionNavigationEventArgs e)
        {
            NavigationBarTitle = (string)e.NavigationContext.Parameters["title"];
        }
    }
}
