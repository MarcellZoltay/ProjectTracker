using Prism.Commands;
using Prism.Mvvm;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class EventDialogViewModel : BindableBase
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

        private EventDialog view;

        public string Text { get; set; }

        private DateTime? startTime;
        public DateTime? StartTime
        {
            get { return startTime; }
            set
            {
                SetProperty(ref startTime, value);

                if (EndTime == null || value > EndTime)
                    EndTime = value;

                MinimumEndTime = value;
            }
        }

        private DateTime? endTime;
        public DateTime? EndTime
        {
            get { return endTime; }
            set { SetProperty(ref endTime, value); }
        }

        private DateTime? minimumEndTime;
        public DateTime? MinimumEndTime
        {
            get { return minimumEndTime; }
            set { SetProperty(ref minimumEndTime, value); }
        }

        public EventDialogViewModel(string text = null, DateTime? startTime = null, DateTime? endTime = null)
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
                StartTime = startTime;
                EndTime = endTime;
            }
        }

        public bool ShowDialog()
        {
            view = new EventDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void AddSaveOnClick()
        {
            var textChecked = Text != "";
            var startTimeChecked = StartTime != null;
            var endTimeChecked = EndTime != null;

            if (textChecked && startTimeChecked && endTimeChecked)
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
