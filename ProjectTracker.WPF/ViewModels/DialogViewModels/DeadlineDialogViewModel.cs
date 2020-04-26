using Prism.Commands;
using Prism.Mvvm;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class DeadlineDialogViewModel : BindableBase
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

        private DeadlineDialog view;

        public string Text { get; set; }

        private DateTime? time;
        public DateTime? Time
        {
            get { return time; }
            set { SetProperty(ref time, value); }
        }

        public DeadlineDialogViewModel(string text = null, DateTime? time = null)
        {
            AddSaveCommand = new DelegateCommand(AddSaveOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);

            if (text == null)
            {
                ButtonContent = "Add";
                Text = "";
            }
            else
            {
                ButtonContent = "Save";
                Text = text;
                Time = time;
            }
        }

        public bool ShowDialog()
        {
            view = new DeadlineDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void AddSaveOnClick()
        {
            var textChecked = Text != "";
            var timeChecked = Time != null;

            if (textChecked && timeChecked)
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
