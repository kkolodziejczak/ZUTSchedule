using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            // Set Data context
            DataContext = new MainWindowViewModel();
        }

        private void Refresh(object sender, RoutedEventArgs e)
        {
            IoC.Settings.RefreshSchedule();
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement)
            {
                uiElement.DragOnClick(e);
            }
        }

        private void QuitClicked(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(Path.GetDirectoryName(Storage.SettingsFilePath)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(Storage.SettingsFilePath));
            }

            XmlSerializer xs = new XmlSerializer(typeof(Storage));
            using (TextWriter tw = new StreamWriter(Storage.SettingsFilePath))
            {
                xs.Serialize(tw, IoC.Settings);
            }

            Application.Current.Shutdown();
        }

        private async void LogoutClicked(object sender, RoutedEventArgs e)
        {
            try
            {
                var cred = IoC.CredentialManager.ReadCredential("ZUTSchedule");
                if (cred != null)
                {
                    IoC.CredentialManager.DeleteCredential("ZUTSchedule");
                    Logger.Info("Credentials were cleared after logout");
                }

                if (File.Exists(Storage.SettingsFilePath))
                {
                    File.Delete(Storage.SettingsFilePath);
                    Logger.Info("Settings file was deleted at Logout");
                }
            }
            catch (Exception ex)
            {
                // Do nothing
            }

            await IoC.Navigation.NavigateToLoginPage();
        }


    }
}
