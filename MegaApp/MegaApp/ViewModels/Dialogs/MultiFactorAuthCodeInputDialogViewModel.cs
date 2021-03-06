﻿using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using MegaApp.Enums;
using MegaApp.Services;

namespace MegaApp.ViewModels.Dialogs
{
    public class MultiFactorAuthCodeInputDialogViewModel : InputDialogViewModel
    {
        public MultiFactorAuthCodeInputDialogViewModel()
        {
            this.Settings = new InputDialogSettings
            {
                InputScopeValue = InputScopeNameValue.NumericPin,
                MaxLength = 6, MinLength = 6
            };
        }

        #region Properties

        private string _digit1;
        /// <summary>
        /// Digit 1 of the MFA code
        /// </summary>
        public string Digit1
        {
            get { return _digit1; }
            set
            {
                if (!SetField(ref _digit1, value)) return;
                this.OnDigitChanged();
            }
        }

        private string _digit2;
        /// <summary>
        /// Digit 2 of the MFA code
        /// </summary>
        public string Digit2
        {
            get { return _digit2; }
            set
            {
                if (!SetField(ref _digit2, value)) return;
                this.OnDigitChanged();
            }
        }

        private string _digit3;
        /// <summary>
        /// Digit 3 of the MFA code
        /// </summary>
        public string Digit3
        {
            get { return _digit3; }
            set
            {
                if (!SetField(ref _digit3, value)) return;
                this.OnDigitChanged();
            }
        }

        private string _digit4;
        /// <summary>
        /// Digit 4 of the MFA code
        /// </summary>
        public string Digit4
        {
            get { return _digit4; }
            set
            {
                if (!SetField(ref _digit4, value)) return;
                this.OnDigitChanged();
            }
        }

        private string _digit5;
        /// <summary>
        /// Digit 5 of the MFA code
        /// </summary>
        public string Digit5
        {
            get { return _digit5; }
            set
            {
                if (!SetField(ref _digit5, value)) return;
                this.OnDigitChanged();
            }
        }

        private string _digit6;
        /// <summary>
        /// Digit 6 of the MFA code
        /// </summary>
        public string Digit6
        {
            get { return _digit6; }
            set
            {
                if (!SetField(ref _digit6, value)) return;
                this.OnDigitChanged();
            }
        }

        private Brush _digitColor;
        /// <summary>
        /// Color for the MFA code digits
        /// </summary>
        public Brush DigitColor
        {
            get { return _digitColor; }
            set { SetField(ref _digitColor, value); }
        }

        /// <summary>
        /// Text introduced in the input dialog
        /// </summary>
        public override string InputText
        {
            get { return base.InputText; }
            set
            {
                base.InputText = value;
                if (!this.IsValidVerifyCode) return;
                this.PrimaryButtonAction();
            }
        }

        /// <summary>
        /// Indicates if the typed verify code has the right format and can be verified
        /// </summary>
        public bool IsValidVerifyCode => !string.IsNullOrWhiteSpace(this.InputText) &&
            this.InputText.Length == 6 && this.InputState == InputState.Normal;

        private bool _showLostDeviceLink;
        /// <summary>
        /// Indicates if show the lost device link or not.
        /// </summary>
        public bool ShowLostDeviceLink
        {
            get { return _showLostDeviceLink; }
            set
            {
                SetField(ref _showLostDeviceLink, value);
                OnPropertyChanged(nameof(this.VariableGridHeight),
                    nameof(this.MessageMargin), nameof(this.DigitsPanelMargin),
                    nameof(this.LostAuthDeviceLinkMargin));
            }
        }

        /// <summary>
        /// Sets the height of the dialog dynamically depending on if 
        /// it shows the lost device link or not.
        /// </summary>
        public int VariableGridHeight => this.ShowLostDeviceLink ?
            (DeviceService.IsPhoneDevice() ? 48 : 72) : 32;

        /// <summary>
        /// Margins of the message of the dialog depending on the device type
        /// </summary>
        public Thickness MessageMargin => DeviceService.IsPhoneDevice() && this.ShowLostDeviceLink ?
            new Thickness(0, 12, 0, 12) : new Thickness(0, 16, 0, 16);

        /// <summary>
        /// Margins of the digits panel of the dialog depending on the device type
        /// </summary>
        public Thickness DigitsPanelMargin => DeviceService.IsPhoneDevice() && this.ShowLostDeviceLink ?
            new Thickness(0, 0, 0, 12) : new Thickness(0, 0, 0, 16);

        /// <summary>
        /// Margins of the "lost authentication device" link of the dialog depending on the device type
        /// </summary>
        public Thickness LostAuthDeviceLinkMargin => DeviceService.IsPhoneDevice() && this.ShowLostDeviceLink ?
            new Thickness(0, 12, 0, 0) : new Thickness(0, 16, 0, 0);

        #endregion

        #region Methods

        /// <summary>
        /// Actions to do when a code digit of the typed MFA is changed
        /// </summary>
        private void OnDigitChanged()
        {
            this.DigitColor = (SolidColorBrush)Application.Current.Resources["SystemControlForegroundBaseHighBrush"];

            this.InputText = string.Format("{0}{1}{2}{3}{4}{5}",
                this.Digit1, this.Digit2, this.Digit3, this.Digit4, this.Digit5, this.Digit6);
        }

        #endregion

        #region AppResources

        public Uri RecoveryUri => new Uri(ResourceService.AppResources.GetString("AR_RecoveryUri"));

        #endregion

        #region UiResources

        public string LostAuthDeviceQuestionText => ResourceService.UiResources.GetString("UI_LostAuthDeviceQuestion");

        #endregion
    }
}
