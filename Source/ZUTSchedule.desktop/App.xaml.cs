using Autofac;
using System.Windows;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            LoadSettings();

            ApplicationSetup();

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        private void ApplicationSetup()
        {
            // setup IoC container
            IoC.Builder.RegisterInstance(new Storage(5,3,50));
            IoC.Builder.RegisterInstance(new NavigationService()).As<INavigationService>();
            IoC.Builder.RegisterInstance(new CredentialManager()).As<ICredentialManager>();
            IoC.Builder.RegisterInstance(new NewsFactory(new INewsService[]
            {
                new WINewsService(),
                new ZUTNewsService(),
            })).As<INewsFactory>();

            IoC.Setup();
        }

        private async void LoadSettings()
        {
            //TODO: Load settings

            
        }



    }
}
