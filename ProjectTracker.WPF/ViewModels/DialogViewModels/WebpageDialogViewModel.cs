using Prism.Commands;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class WebpageDialogViewModel
    {
        public DelegateCommand SaveCommand { get; }
        public DelegateCommand CancelCommand { get; }

        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            private set
            {
                dialogResult = value;
                view.DialogResult = dialogResult;
            }
        }

        private WebpageDialog view;

        public string WebpageLink { get; set; }

        public WebpageDialogViewModel()
        {
            SaveCommand = new DelegateCommand(SaveOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);
        }


        public bool? ShowDialog()
        {
            view = new WebpageDialog();
            view.DataContext = this;
            view.ShowDialog();

            return DialogResult;
        }

        private void SaveOnClick()
        {
            var linkChecked = WebpageLink != null && WebpageLink != "";

            if (linkChecked)
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
