﻿using System;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml.Controls;
using MegaApp.Classes;
using MegaApp.MegaApi;
using MegaApp.Services;
using MegaApp.ViewModels.Contacts;
using MegaApp.Views.Dialogs;

namespace MegaApp.ViewModels.UserControls
{
    public class ShareToPanelViewModel : BaseUiViewModel
    {
        public ShareToPanelViewModel()
        {
            this.AddContactAndShareCommand = new RelayCommand(AddContactAndShare);
            this.CancelCommand = new RelayCommand(Cancel);
            this.ConfirmShareCommand = new RelayCommand(ConfirmShare);
        }

        #region Events

        /// <summary>
        /// Event triggered when user tap the cancel/close button.
        /// </summary>
        public EventHandler ClosePanelEvent;

        /// <summary>
        /// Event invocator method called when user tap the cancel/close button.
        /// </summary>
        protected virtual void OnClosePanelEvent()
        {
            ClosePanelEvent?.Invoke(this, EventArgs.Empty);
            this.MegaContacts.ItemCollection.SelectedItems.Clear();
        }

        #endregion

        #region Commands

        public ICommand AddContactAndShareCommand { get; }
        public ICommand CancelCommand { get; }
        public ICommand ConfirmShareCommand { get; }

        #endregion

        #region Methods

        private async void AddContactAndShare()
        {
            // Ask user for the access level
            var shareFolderToDialog = new ShareFolderToDialog(this.Node.Name);
            var dialogResult = await shareFolderToDialog.ShowAsync();
            if (dialogResult != ContentDialogResult.Primary) return;

            var share = new ShareRequestListenerAsync();
            var result = await share.ExecuteAsync(() =>
            {
                SdkService.MegaSdk.shareByEmail(this.Node.OriginalMNode,
                    shareFolderToDialog.ViewModel.ContactEmail,
                    (int)shareFolderToDialog.ViewModel.AccessLevel, share);
            });

            if (!result)
            {
                OnUiThread(async () =>
                {
                    await DialogService.ShowAlertAsync(
                        ResourceService.AppMessages.GetString("AM_ShareFolderFailed_Title"),
                        ResourceService.AppMessages.GetString("AM_ShareFolderFailed"));
                });
            }
        }

        private void Cancel()
        {
            this.OnClosePanelEvent();
        }

        private async void ConfirmShare()
        {
            if (!MegaContacts.ItemCollection.HasSelectedItems) return;

            // Ask user for the access level
            var shareFolderToDialog = new SetSharedFolderPermissionDialog();
            var dialogResult = await shareFolderToDialog.ShowAsync();
            if (dialogResult != ContentDialogResult.Primary) return;

            // Use a temp variable to avoid "InvalidOperationException" tracing the selected items
            var selectedItems = this.MegaContacts.ItemCollection.SelectedItems.ToList();

            this.OnClosePanelEvent();

            bool result = true;
            foreach (var contact in selectedItems)
            {
                var share = new ShareRequestListenerAsync();
                result = result & await share.ExecuteAsync(() =>
                {
                    SdkService.MegaSdk.share(this.Node.OriginalMNode,
                        contact.MegaUser, (int)shareFolderToDialog.ViewModel.AccessLevel, share);
                });
            }

            if (!result)
            {
                OnUiThread(async () =>
                {
                    await DialogService.ShowAlertAsync(
                        ResourceService.AppMessages.GetString("AM_ShareFolderFailed_Title"),
                        ResourceService.AppMessages.GetString("AM_ShareFolderFailed"));
                });
            }
        }

        #endregion

        #region Properties

        public ContactsListViewModel MegaContacts => ContactsService.MegaContacts;

        private NodeViewModel _node;
        public NodeViewModel Node
        {
            get { return _node; }
            set { SetField(ref _node, value); }
        }

        #endregion

        #region UiResources

        public string AddContactText => ResourceService.UiResources.GetString("UI_AddContact");
        public string AllContactsText => ResourceService.UiResources.GetString("UI_AllContacts");
        public string CancelText => ResourceService.UiResources.GetString("UI_Cancel");
        public string ConfirmShareText => ResourceService.UiResources.GetString("UI_ConfirmShare");
        public string MultiSelectText => ResourceService.UiResources.GetString("UI_MultiSelect");
        public string ShareToText => ResourceService.UiResources.GetString("UI_ShareTo");

        #endregion

        #region VisualResources
        
        public string CancelPathData => ResourceService.VisualResources.GetString("VR_CancelPathData");
        public string ConfirmPathData => ResourceService.VisualResources.GetString("VR_ConfirmPathData");
        public string MultiSelectPathData => ResourceService.VisualResources.GetString("VR_MultiSelectPathData");

        #endregion
    }
}
