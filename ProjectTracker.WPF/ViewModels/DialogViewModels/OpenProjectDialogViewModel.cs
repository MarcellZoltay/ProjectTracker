using Prism.Commands;
using Prism.Mvvm;
using ProjectTracker.BLL.Models;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectTracker.WPF.ViewModels.DialogViewModels
{
    public class OpenProjectDialogViewModel : BindableBase
    {
        public DelegateCommand OpenCommand { get; }
        public DelegateCommand CancelCommand { get; }
        public DelegateCommand<bool?> SelectAllCommand { get; }
        public DelegateCommand CheckAllSelectionCommand { get; }

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

        private OpenProjectDialog view;

        public List<PathToOpen> Paths { get; }

        private bool allSelected;
        public bool AllSelected
        {
            get { return allSelected; }
            set { SetProperty(ref allSelected, value); }
        }

        public OpenProjectDialogViewModel(IEnumerable<PathListViewItem> paths, string type)
        {
            Paths = new List<PathToOpen>(paths.ConvertToPathToOpenItems());

            OpenCommand = new DelegateCommand(OpenOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);
            SelectAllCommand = new DelegateCommand<bool?>(SelectAll);
            CheckAllSelectionCommand = new DelegateCommand(CheckAllSelection);

            if (type == null)
            {
                SelectAll(true);
            }
            else
            {
                SelectType(type);
            }
        }

        public bool? ShowDialog()
        {
            view = new OpenProjectDialog(this);
            view.ShowDialog();

            return DialogResult;
        }

        private void OpenOnClick()
        {
            DialogResult = true;
        }
        private void CancelOnClick()
        {
            DialogResult = false;
        }

        private void SelectAll(bool? isChecked)
        {
            foreach (var item in Paths)
            {
                item.Open = isChecked.Value;
            }

            CheckAllSelection();
        }
        private void SelectType(string type)
        {
            foreach (var item in Paths)
            {
                if(item.Path.Type == type)
                    item.Open = true;
            }

            CheckAllSelection();
        }
        private void CheckAllSelection()
        {
            foreach (var item in Paths)
            {
                if (item.Open == false)
                {
                    AllSelected = false;

                    return;
                }
            }

            if (Paths.Count != 0)
                AllSelected = true;
        }

    }
}
