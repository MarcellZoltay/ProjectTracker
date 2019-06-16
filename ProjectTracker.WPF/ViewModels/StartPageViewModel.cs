﻿using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Threading;
using Unity;

namespace ProjectTracker.WPF.ViewModels
{
    public class StartPageViewModel : BindableBase
    {
        private readonly IRegionManager regionManager;

        public DelegateCommand CreateProjectCommand { get; }
        public DelegateCommand<Project> OpenProjectCommand { get; }
        public DelegateCommand<Project> RenameProjectCommand { get; }
        public DelegateCommand<Project> DeleteProjectCommand { get; }
        public DelegateCommand MouseLeftButtonDownCommand { get; }
        public DelegateCommand MouseRightButtonDownCommand { get; }


        private IProjectService projectService;
        public ObservableCollection<Project> Projects { get; }

        private Project selectedProject;
        public Project SelectedProject
        {
            get { return selectedProject; }
            set { SetProperty(ref selectedProject, value); }
        }


        private bool leftMouseButtonClicked;

        private bool isLoaded = true;
        public bool IsLoaded
        {
            get { return isLoaded; }
            set { SetProperty(ref isLoaded, value); }
        }


        public StartPageViewModel(IRegionManager regionManager, IProjectService projectService)
        {
            this.regionManager = regionManager;

            CreateProjectCommand = new DelegateCommand(CreateProject);
            OpenProjectCommand = new DelegateCommand<Project>(OpenProject);
            RenameProjectCommand = new DelegateCommand<Project>(RenameProject);
            DeleteProjectCommand = new DelegateCommand<Project>(DeleteProject);
            MouseLeftButtonDownCommand = new DelegateCommand(MouseLeftButtonDown);
            MouseRightButtonDownCommand = new DelegateCommand(MouseRightButtonDown);

            Projects = new ObservableCollection<Project>();

            this.projectService = projectService;

            var currentDispatcher = Dispatcher.CurrentDispatcher;
            Task.Run(() =>
            {
                var projects = projectService.GetProjects();

                currentDispatcher.Invoke(new Action(() =>
                {
                    Projects.AddRange(projects);
                    IsLoaded = false;
                }));
            });
        }

        
        private void CreateProject()
        {
            var dialogViewModel = new ProjectDialogViewModel();
            if (dialogViewModel.ShowDialog() == true)
            {
                var project = projectService.CreateProject(dialogViewModel.ProjectTitle);
                Projects.Add(project);
            }

            SelectedProject = null;
        }
        private void OpenProject(Project project)
        {
            if (leftMouseButtonClicked && project != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("projectTitle", project.Title);

                regionManager.RequestNavigate("MainRegion", "ProjectPage", navigationParameters);

                SelectedProject = null;
            }
        }
        private async void RenameProject(Project project)
        {
            var dialogViewModel = new ProjectDialogViewModel(project.Title);
            if (dialogViewModel.ShowDialog() == true)
            {
                project.Title = dialogViewModel.ProjectTitle;
                await projectService.UpdateProjectAsync(project);
            }

            SelectedProject = null;
        }
        private async void DeleteProject(Project project)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {project.Title}?", "Delete project", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if(result == MessageBoxResult.OK)
            {
                Projects.Remove(project);
                await projectService.DeleteProjectAsync(project);
            }

            SelectedProject = null;
        }

        private void MouseLeftButtonDown()
        {
            leftMouseButtonClicked = true;
        }
        private void MouseRightButtonDown()
        {
            leftMouseButtonClicked = false;
        }

    }
}
