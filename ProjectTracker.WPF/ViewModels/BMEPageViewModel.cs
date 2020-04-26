using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.Constants;
using ProjectTracker.WPF.HelperClasses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace ProjectTracker.WPF.ViewModels
{
    public class BMEPageViewModel : BindableBase, INavigationAware
    {
        private string title;
        public string Title
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        private readonly IRegionManager regionManager;

        private string pageTitle;
        public string PageTitle
        {
            get { return pageTitle; }
            set { SetProperty(ref pageTitle, value); }
        }

        public DelegateCommand<TermListViewItem> OpenTermCommand { get; }
        public DelegateCommand<Course> OpenCourseCommand { get; }


        public ObservableCollection<TermListViewItem> TermsListViewItems { get; }

        private Course selectedCourse;
        public Course SelectedCourse
        {
            get { return selectedCourse; }
            set { SetProperty(ref selectedCourse, value); }
        }

        private bool isLoading = true;
        public bool IsLoading
        {
            get { return isLoading; }
            set { SetProperty(ref isLoading, value); }
        }

        public BMEPageViewModel(IRegionManager regionManager, ITermService termService)
        {
            this.regionManager = regionManager;

            OpenTermCommand = new DelegateCommand<TermListViewItem>(OpenTerm);
            OpenCourseCommand = new DelegateCommand<Course>(OpenCourse);

            TermsListViewItems = new ObservableCollection<TermListViewItem>();

            var currentDispatcher = Dispatcher.CurrentDispatcher;
            var taskTerms = Task.Run(() =>
            {
                Exception e = null;
                MessageBoxResult result = MessageBoxResult.None;
                do
                {
                    try
                    {
                        var terms = termService.GetTerms();

                        currentDispatcher.Invoke(new Action(() =>
                        {
                            TermsListViewItems.AddRange(terms.ConvertToTermListViewItems());

                            IsLoading = false;
                        }));

                        e = null;
                    }
                    catch (Exception exception)
                    {
                        e = exception;
                        result = MessageBox.Show(e.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Information);
                    }
                }
                while (e != null && result == MessageBoxResult.OK);
            });
        }

        private void OpenTerm(TermListViewItem termListViewItem)
        {
            if (termListViewItem != null)
            {
                PageTitle = termListViewItem.Term.Title;

                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("termListViewItem", termListViewItem);

                regionManager.RequestNavigate(RegionNames.SubjectRegion, PageNames.TermPage, navigationParameters);
            }
        }

        private void OpenCourse(Course course)
        {
            if (course != null)
            {
                PageTitle = course.Title;

                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("project", course.Project);

                regionManager.RequestNavigate(RegionNames.SubjectRegion, PageNames.ProjectPage, navigationParameters);

                SelectedCourse = null;
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            Title = (string)navigationContext.Parameters["title"];
        }
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
            // intentionally empty
        }
        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }
    }
}
