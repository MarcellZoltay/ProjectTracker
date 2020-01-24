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
    public class TodoDialogViewModel : BindableBase
    {
        public string ButtonContent { get; }

        public DelegateCommand AddSaveCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand RemoveDeadlineCommand { get; }

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

        private TodoDialog view;

        public string Title { get; }
        public string Text { get; set; }

        private DateTime? deadline;
        public DateTime? Deadline
        {
            get { return deadline; }
            set { SetProperty(ref deadline, value); }
        }

        public TodoDialogViewModel(string title, string text = null, DateTime? deadline = null)
        {
            AddSaveCommand = new DelegateCommand(AddSaveOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);
            RemoveDeadlineCommand = new DelegateCommand(RemoveDeadlineOnClick);

            Title = title ?? "";

            if (text == null)
            {
                ButtonContent = "Add";    
                Text = "";
            }
            else
            {
                ButtonContent = "Save";
                Text = text;
            }

            if (deadline != null)
            {
                Deadline = deadline;
            }
        }

        public bool ShowDialog()
        {
            view = new TodoDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void AddSaveOnClick()
        {
            var textChecked = Text != "";

            if (textChecked)
            {
                DialogResult = true;
            }
        }

        private void CancelOnClick()
        {
            DialogResult = false;
        }

        private void RemoveDeadlineOnClick()
        {
            Deadline = null;
        }
    }
}
