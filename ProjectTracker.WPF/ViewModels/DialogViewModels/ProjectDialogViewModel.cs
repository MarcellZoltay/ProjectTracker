using Prism.Commands;
using Prism.Mvvm;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class ProjectDialogViewModel : BindableBase
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

        private ProjectDialog view;


        public string ProjectTitle { get; set; }

        public ProjectDialogViewModel(string projectTitle = null)
        {
            AddSaveCommand = new DelegateCommand(AddSaveOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);

            if (projectTitle == null)
            {
                ButtonContent = "Add";
            }
            else
            {
                ButtonContent = "Save";

                ProjectTitle = projectTitle;
            }
        }


        public bool ShowDialog()
        {
            view = new ProjectDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void AddSaveOnClick()
        {
            var titleChecked = ProjectTitle != "";

            if (titleChecked)
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
