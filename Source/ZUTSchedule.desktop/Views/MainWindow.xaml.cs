using System;
using System.Windows;
using System.Windows.Input;
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

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            if (sender is UIElement uiElement)
            {
                uiElement.DragOnClick(e);
            }
        }

        private void QuitClicked(object sender, RoutedEventArgs e)
        {
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
