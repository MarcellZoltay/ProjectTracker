using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.WPF.ViewModels
{
    public class ProjectPageViewModel : BindableBase, INavigationAware
    {
        private readonly IRegionManager regionManager;

        private string projectTitle;
        public string ProjectTitle
        {
            get { return projectTitle; }
            set { SetProperty(ref projectTitle, value); }

        }

        public DelegateCommand BackToStartPageClickCommand { get; }

        public ProjectPageViewModel(IRegionManager regionManager)
        {
            this.regionManager = regionManager;

            BackToStartPageClickCommand = new DelegateCommand(BackToStartPageClick);
        }

        private void BackToStartPageClick()
        {
            regionManager.RequestNavigate("MainRegion", "StartPage");
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            ProjectTitle = navigationContext.Parameters["projectTitle"].ToString();
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // empty
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
    }
}
