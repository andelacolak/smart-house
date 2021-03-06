﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;
using Template10.Common;
using Template10.Mvvm;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using SmartHouse.UWPClient.BackgroundTasks;
using Template10.Utils;
using SmartHouse.UWPLib.Service;

namespace SmartHouse.UWPClient.ViewModels
{
    public class SettingsPageViewModel : BaseViewModel
    {
        public SettingsPartViewModel SettingsPartViewModel { get; } = new SettingsPartViewModel();
        public AboutPartViewModel AboutPartViewModel { get; } = new AboutPartViewModel();

        public override Task OnNavigatedFromAsync(IDictionary<string, object> pageState, bool suspending)
        {
            if(!string.IsNullOrWhiteSpace(SettingsPartViewModel.Username) && !string.IsNullOrWhiteSpace(SettingsPartViewModel.Password))
            {
                var settings = SettingsService.Instance;
                settings.SaveUsernamePassword(SettingsPartViewModel.Username, SettingsPartViewModel.Password);
            }

            return base.OnNavigatedFromAsync(pageState, suspending);
        }
    }

    public class SettingsPartViewModel : BaseViewModel
    {
        private readonly SettingsService _settings;

        public SettingsPartViewModel()
        {
            if (Windows.ApplicationModel.DesignMode.DesignModeEnabled)
            {
                // designtime
            }
            else
            {
                _settings = SettingsService.Instance;
                var credentials = _settings.GetCredentialFromLocker();

                if(credentials != null)
                {
                    Username = credentials.UserName;
                    Password = credentials.Password;
                }
            }
        }

        public bool UseBackgroundWorker
        {
            get => _settings.UseBackgroundWorker;
            set { _settings.UseBackgroundWorker = value; base.RaisePropertyChanged(); }
        }

        public bool UseShellBackButton
        {
            get => _settings.UseShellBackButton;
            set
            {
                _settings.UseShellBackButton = value;
                base.RaisePropertyChanged();

                BootStrapper.Current.NavigationService.Dispatcher.Dispatch(() =>
                {
                    BootStrapper.Current.ShowShellBackButton = value;
                    BootStrapper.Current.UpdateShellBackButton();
                    BootStrapper.Current.NavigationService.Refresh();
                });
            }
        }

        public string WebHost
        {
            get => _settings.WebHost;
            set => _settings.WebHost = value;
        }

        public string HostIP
        {
            get => _settings.HostIP;
            set => _settings.HostIP = value;
        }

        public string HostPort
        {
            get => _settings.HostPort;
            set => _settings.HostPort = value;
        }

        public string Password
        {
            get => Get<string>();
            set => Set(value);
        }

        public string Username
        {
            get => Get<string>();
            set => Set(value);
        }

        public bool LiveTile
        {
            get => _settings.LiveTile;
            set => _settings.LiveTile = value;
        }

        public bool PhoneTask
        {
            get => _settings.PhoneTask;
            set
            {
                _settings.PhoneTask = value;
                if(value)
                    EnablePhoneTask();
                else
                    DisablePhoneTask();
            }
        }

        public Visibility PhoneTaskVisibility =>
            ApiInformation.IsTypePresent("Windows.ApplicationModel.Background.PhoneTrigger")
                ? Visibility.Visible
                : Visibility.Collapsed;

        public bool UseLightThemeButton
        {
            get => _settings.AppTheme.Equals(ApplicationTheme.Light);
            set
            {
                _settings.AppTheme = value ? ApplicationTheme.Light : ApplicationTheme.Dark; base.RaisePropertyChanged();

                (Window.Current.Content as FrameworkElement).RequestedTheme = _settings.AppTheme.ToElementTheme();
                Views.Shell.HamburgerMenu.RefreshStyles(_settings.AppTheme);
            }
        }

        private void EnablePhoneTask()
        {
            var task = new PhoneCallTask();
            task.RegisterBackgroundTask();
        }

        private void DisablePhoneTask()
        {
            var task = new PhoneCallTask();
            task.UnRegisterBackgroundTask();
        }
    }

    public class AboutPartViewModel : BaseViewModel
    {
        public Uri Logo => Windows.ApplicationModel.Package.Current.Logo;

        public string DisplayName => Windows.ApplicationModel.Package.Current.DisplayName;

        public string Publisher => Windows.ApplicationModel.Package.Current.PublisherDisplayName;

        public string Version
        {
            get
            {
                var v = Windows.ApplicationModel.Package.Current.Id.Version;
                return $"{v.Major}.{v.Minor}.{v.Build}.{v.Revision}";
            }
        }

        public Uri RateMe => new Uri("http://aka.ms/template10");
    }


}
