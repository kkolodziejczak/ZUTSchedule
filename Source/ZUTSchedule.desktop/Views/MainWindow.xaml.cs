using System;
using System.IO;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
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
                IoC.CredentialManager.DeleteCredential("ZUTSchedule");
            }
            catch (Exception ex)
            {
                // Do nothing
            }
            await IoC.Navigation.NavigateToLoginPage();
        }


    }
}
