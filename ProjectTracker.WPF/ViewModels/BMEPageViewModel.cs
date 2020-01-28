using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using ProjectTracker.BLL.Models;
using ProjectTracker.BLL.Services.Interfaces;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.ViewModels.DialogViewModels;
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
		public DelegateCommand GoBackClickCommand { get; }
		public DelegateCommand<Course> OpenCourseCommand { get; }
		public DelegateCommand<TermListViewItem> AddCourseCommand { get; }
		public DelegateCommand<Course> EditCourseCommand { get; }
		public DelegateCommand<Course> DeleteCourseCommand { get; }

		private ITermService termService;
		private ICourseService courseService;

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

		public BMEPageViewModel(IRegionManager regionManager, ITermService termService, ICourseService courseService)
		{
			this.regionManager = regionManager;
			this.termService = termService;
			this.courseService = courseService;

			GoBackClickCommand = new DelegateCommand(GoBackCommand);
			OpenCourseCommand = new DelegateCommand<Course>(OpenCourse);
			AddCourseCommand = new DelegateCommand<TermListViewItem>(AddCourse);
			EditCourseCommand = new DelegateCommand<Course>(EditCourse);
			DeleteCourseCommand = new DelegateCommand<Course>(DeleteCourse);

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

		private void GoBackCommand()
		{
			regionManager.RequestNavigate("MainRegion", "StartPage");
		}

		private void OpenCourse(Course course)
		{
			if (course != null)
			{
				var navigationParameters = new NavigationParameters();
				navigationParameters.Add("project", course.Project);

				regionManager.RequestNavigate("MainRegion", "ProjectPage", navigationParameters);

				SelectedCourse = null;
			}
		}
		private void AddCourse(TermListViewItem termListViewItem)
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
				var termListViewItem = TermsListViewItems.Where(p => p.Courses.Contains(course)).First();

				termListViewItem.RemoveCourse(course);

				await courseService.DeleteCourseAsync(course);
			}

			SelectedCourse = null;
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
