using Prism.Commands;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class ApplicationDialogViewModel
    {
        public DelegateCommand AddCommand { get; }
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

        private ApplicationDialog view;

        public string AppPath { get; set; }

        public ApplicationDialogViewModel()
        {
            AddCommand = new DelegateCommand(AddOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);
        }

        public bool ShowDialog()
        {
            view = new ApplicationDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void AddOnClick()
        {
            var pathChecked = AppPath != null && AppPath != "";

            if (pathChecked)
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
