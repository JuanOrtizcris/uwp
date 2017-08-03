﻿using System;
using System.Windows.Input;
using MegaApp.Classes;
using MegaApp.Enums;
using MegaApp.Services;
using MegaApp.Views;

namespace MegaApp.ViewModels.MyAccount
{
    public class MyAccountBaseViewModel : BaseUiViewModel
    {
        public EventHandler GoToUpgrade;

        public MyAccountBaseViewModel()
        {
            this.RubbishBinCommand = new RelayCommand(RubbishBin);
            this.UpgradeCommand = new RelayCommand(Upgrade);
        }

        #region Commands

        public ICommand RubbishBinCommand { get; }
        public ICommand UpgradeCommand { get; }

        #endregion

        #region Private Methods

        private void RubbishBin()
        {
            OnUiThread(() =>
            {
                NavigateService.Instance.Navigate(typeof(CloudDrivePage), false,
                    NavigationObject.Create(typeof(GeneralViewModel), NavigationActionType.RubbishBin));
            });
        }

        private void Upgrade()
        {
            GoToUpgrade?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region Properties

        public AccountDetailsViewModel AccountDetails => AccountService.AccountDetails;
        public UserDataViewModel UserData => AccountService.UserData;

        #endregion

        #region UiResources

        public string StorageOverquotaWarningText => ResourceService.UiResources.GetString("UI_StorageOverquotaWarningText");
        public string TransferOverquotaWarningText => ResourceService.UiResources.GetString("UI_TransferOverquotaWarningText");
        public string UpgradeText => ResourceService.UiResources.GetString("UI_Upgrade");
        public string RubbishBinText => ResourceService.UiResources.GetString("UI_RubbishBinName");
        public string PleaseWaitText => ResourceService.UiResources.GetString("UI_PleaseWait");

        #endregion

        #region VisualResources

        public string NoteIconPathData => ResourceService.VisualResources.GetString("VR_NoteIconPathData");
        public string WarningIconPathData => ResourceService.VisualResources.GetString("VR_WarningIconPathData");

        #endregion
    }
}