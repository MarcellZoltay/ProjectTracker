using Prism.Commands;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class CourseDialogViewModel
    {
        public string ButtonContent { get; }

        public DelegateCommand AddSaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private bool dialogResult;
        public bool DialogResult
        {
            get { return dialogResult; }
            private set
            {
                dialogResult = value;
                view.DialogResult = dialogResult;
            }
        }

        private CourseDialog view;

        public string CourseTitle { get; set; }
        public int Credit { get; set; }

        public CourseDialogViewModel(string title = null, int? credit = null)
        {
            AddSaveCommand = new DelegateCommand(AddSaveOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);

            if (title == null)
            {
                ButtonContent = "Add";
            }
            else
            {
                ButtonContent = "Save";
                CourseTitle = title;
            }

            if (credit.HasValue)
            {
                Credit = credit.Value;
            }
        }

        public bool ShowDialog()
        {
            view = new CourseDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void AddSaveOnClick()
        {
            var courseNameChecked = CourseTitle != null && CourseTitle != "";

            if (courseNameChecked)
            {
                DialogResult = true;
            }
        }

        private void CancelOnClick()
        {
            DialogResult = false;
        }
    }
}
