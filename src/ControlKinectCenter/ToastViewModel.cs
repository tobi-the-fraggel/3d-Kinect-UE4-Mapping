using System;
using System.ComponentModel;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Core;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;

namespace ControlKinectCenter
{
    public class ToastViewModel
    {
        private readonly Notifier _notifier;

        public ToastViewModel()
        {
            Properties.Settings.Default.PropertyChanged += ToastPropertyChanged;

            _notifier = new Notifier(cfg =>
                {
                    cfg.PositionProvider = new PrimaryScreenPositionProvider(
                    corner: Corner.BottomRight,
                    offsetX: 25,
                    offsetY: 100);

                    cfg.LifetimeSupervisor = new TimeAndCountBasedLifetimeSupervisor(
                    notificationLifetime: TimeSpan.FromSeconds(5),
                    maximumNotificationCount: MaximumNotificationCount.FromCount(6));

                    cfg.Dispatcher = Application.Current.Dispatcher;

                    cfg.DisplayOptions.TopMost = true;
                    cfg.DisplayOptions.Width = 250;
                });

            _notifier.ClearMessages();
        }

        private void ToastPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (Properties.Settings.Default.ShowToast)
            {
                if (e.PropertyName == "NotifyInfo" && Properties.Settings.Default.NotifyInfo != "")
                {
                    ShowInformation(Properties.Settings.Default.NotifyInfo);
                    Properties.Settings.Default.NotifyInfo = "";
                    Properties.Settings.Default.Save();
                }
                else if (e.PropertyName == "NotifyWarning" && Properties.Settings.Default.NotifyWarning != "")
                {
                    ShowWarning(Properties.Settings.Default.NotifyWarning);
                    Properties.Settings.Default.NotifyWarning = "";
                    Properties.Settings.Default.Save();
                }
                else if (e.PropertyName == "NotifyError" && Properties.Settings.Default.NotifyError != "")
                {
                    ShowError(Properties.Settings.Default.NotifyError);
                    Properties.Settings.Default.NotifyError = "";
                    Properties.Settings.Default.Save();
                }
                else if (e.PropertyName == "NotifySuccess" && Properties.Settings.Default.NotifySuccess != "")
                {
                    ShowSuccess(Properties.Settings.Default.NotifySuccess);
                    Properties.Settings.Default.NotifySuccess = "";
                    Properties.Settings.Default.Save();
                }
            }
        }

        public void OnUnloaded()
        {
            _notifier.Dispose();
        }

        public void ShowInformation(string message)
        {
            _notifier.ShowInformation(message);
        }

        public void ShowInformation(string message, MessageOptions opts)
        {
            _notifier.ShowInformation(message, opts);
        }

        public void ShowSuccess(string message)
        {
            _notifier.ShowSuccess(message);
        }

        public void ShowSuccess(string message, MessageOptions opts)
        {
            _notifier.ShowSuccess(message, opts);
        }

        internal void ClearMessages(string msg)
        {
            _notifier.ClearMessages(msg);
        }

        public void ShowWarning(string message)
        {
            _notifier.ShowWarning(message);
        }

        public void ShowWarning(string message, MessageOptions opts)
        {
            _notifier.ShowWarning(message, opts);
        }

        public void ShowError(string message)
        {
            _notifier.ShowError(message);
        }

        public void ShowError(string message, MessageOptions opts)
        {
            _notifier.ShowError(message, opts);
        }
    }
}
