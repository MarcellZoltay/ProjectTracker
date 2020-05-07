using Microsoft.WindowsAPICodePack.Dialogs;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.ViewModels.DialogViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ProjectTracker.WPF.ViewModels
{
    public class TermPageViewModel : BindableBase, INavigationAware
    {
        public DelegateCommand AddCourseCommand { get; }
        public DelegateCommand<Course> EditCourseCommand { get; }
        public DelegateCommand<Course> DeleteCourseCommand { get; }
        public DelegateCommand<Course> IsFulfilledClickedCommand { get; set; }
        public DelegateCommand ImportLessonsAsTodosFromExcelCommand { get; }

        private ITermService termService;
        private ICourseService courseService;

        private TermListViewItem termListViewItem;
        public TermListViewItem TermListViewItem
        {
            get { return termListViewItem; }
            set { SetProperty(ref termListViewItem, value); }
        }

        private Course selectedCourse;
        public Course SelectedCourse
        {
            get { return selectedCourse; }
            set { SetProperty(ref selectedCourse, value); }
        }


        public TermPageViewModel(ITermService termService, ICourseService courseService)
        {
            AddCourseCommand = new DelegateCommand(AddCourse);
            EditCourseCommand = new DelegateCommand<Course>(EditCourse);
            DeleteCourseCommand = new DelegateCommand<Course>(DeleteCourse);
            IsFulfilledClickedCommand = new DelegateCommand<Course>(IsFulfilledClicked);
            ImportLessonsAsTodosFromExcelCommand = new DelegateCommand(ImportLessonsAsTodosFromExcel);

            this.termService = termService;
            this.courseService = courseService;
        }

        private void AddCourse()
        {
            var dialogViewModel = new CourseDialogViewModel();
            if (dialogViewModel.ShowDialog() == true)
            {
                var course = courseService.CreateCourse(termListViewItem.Term.Id, dialogViewModel.CourseTitle, dialogViewModel.Credit);

                termListViewItem.AddCourse(course);
            }
        }
        private async void EditCourse(Course course)
        {
            var dialogViewModel = new CourseDialogViewModel(course.Title, course.Credit);
            if (dialogViewModel.ShowDialog() == true)
            {
                course.Title = dialogViewModel.CourseTitle;
                course.Credit = dialogViewModel.Credit;

                await courseService.UpdateCourseAsync(course);
            }

            SelectedCourse = null;
        }
        private async void DeleteCourse(Course course)
        {
            var result = MessageBox.Show($"Are you sure you want to delete {course.Title}?", "Delete course", MessageBoxButton.OKCancel, MessageBoxImage.Warning);

            if (result == MessageBoxResult.OK)
            {
                termListViewItem.RemoveCourse(course);

                await courseService.DeleteCourseAsync(course);
            }

            SelectedCourse = null;
        }
        private async void IsFulfilledClicked(Course course)
        {
            await courseService.UpdateCourseAsync(course);
        }

        private void ImportLessonsAsTodosFromExcel()
        {
            var commonOpenFileDialog = new CommonOpenFileDialog();

            if (commonOpenFileDialog.ShowDialog() != CommonFileDialogResult.Ok)
                return;

            try
            {
                termService.ImportLessonsAsEventsFromExcel(commonOpenFileDialog.FileName, TermListViewItem.Term);
            }
            catch (Exception e)
            {
                MessageBox.Show($"{e.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            TermListViewItem = (TermListViewItem)navigationContext.Parameters["termListViewItem"];
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
