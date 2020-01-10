using Prism.Commands;
using Prism.Mvvm;
using ProjectTracker.BLL.Models;
using ProjectTracker.WPF.HelperClasses;
using ProjectTracker.WPF.Views.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private bool? dialogResult = false;
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

        public List<PathToOpen> WebpageLinks { get; }
        public List<PathToOpen> FilePaths { get; }
        public List<PathToOpen> FolderPaths { get; }
        public List<PathToOpen> ApplicationPaths { get; }

        private bool allSelected;
        public bool AllSelected
        {
            get { return allSelected; }
            set { SetProperty(ref allSelected, value); }
        }

        public OpenProjectDialogViewModel()
        {
            OpenCommand = new DelegateCommand(OpenOnClick);
            CancelCommand = new DelegateCommand(CancelOnClick);
            SelectAllCommand = new DelegateCommand<bool?>(SelectAll);
            CheckAllSelectionCommand = new DelegateCommand(CheckAllSelection);

            WebpageLinks = new List<PathToOpen>();
            FilePaths = new List<PathToOpen>();
            FolderPaths = new List<PathToOpen>();
            ApplicationPaths = new List<PathToOpen>();
        }

        public bool? ShowDialog()
        {
            view = new OpenProjectDialog(this);

            SelectAll(true);
            AllSelected = true;

            view.ShowDialog();

            return DialogResult;
        }

        public void AddWebpageLinks(IEnumerable<PathListViewItem> webpageLinks)
        {
            WebpageLinks.AddRange(webpageLinks.ConvertToPathToOpenItems());
        }
        public void AddFilePaths(IEnumerable<PathListViewItem> filePaths)
        {
            FilePaths.AddRange(filePaths.ConvertToPathToOpenItems());
        }
        public void AddFolderPaths(IEnumerable<PathListViewItem> Pathfolders)
        {
            FolderPaths.AddRange(Pathfolders.ConvertToPathToOpenItems());
        }
        public void AddApplicationPaths(IEnumerable<PathListViewItem> applicationPaths)
        {
            ApplicationPaths.AddRange(applicationPaths.ConvertToPathToOpenItems());
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
            foreach (var item in WebpageLinks)
            {
                item.Open = isChecked.Value;
            }
            foreach (var item in FilePaths)
            {
                item.Open = isChecked.Value;
            }
            foreach (var item in FolderPaths)
            {
                item.Open = isChecked.Value;
            }
            foreach (var item in ApplicationPaths)
            {
                item.Open = isChecked.Value;
            }
        }
        private void CheckAllSelection()
        {
            foreach (var item in WebpageLinks)
            {
                if (item.Open == false)
                {
                    AllSelected = false;

                    return;
                }
            }
            foreach (var item in FilePaths)
            {
                if (item.Open == false)
                {
                    AllSelected = false;

                    return;
                }
            }
            foreach (var item in FolderPaths)
            {
                if (item.Open == false)
                {
                    AllSelected = false;

                    return;
                }
            }
            foreach (var item in ApplicationPaths)
            {
                if (item.Open == false)
                {
                    AllSelected = false;

                    return;
                }
            }

            AllSelected = true;
        }
    }
}
