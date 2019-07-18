using Prism.Ioc;
using Prism.Modularity;
using System.Windows;
using ProjectTracker.WPF.Views;
using ProjectTracker.WPF.ViewModels;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.BLL.Services.Implementations;

namespace ProjectTracker.WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<StartPage, StartPageViewModel>();
            containerRegistry.RegisterForNavigation<ProjectPage, ProjectPageViewModel>();

            containerRegistry.RegisterSingleton<IProjectService, ProjectService>();
            containerRegistry.RegisterSingleton<ITodoService, TodoService>();
        }
    }
}
