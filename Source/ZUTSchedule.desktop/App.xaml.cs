using Autofac;
using System;
using System.IO;
using System.Windows;
using System.Xml.Serialization;
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

            ApplicationSetup();

            LoadSettings();

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }

        private void ApplicationSetup()
        {
            // setup IoC container
            IoC.Builder.RegisterInstance(new Storage()
            {
                HowManyDaysIsNew = 3,
                HowLongNewsMessages = 50,
                NumberOfDaysInTheWeek = 5,
            });
            IoC.Builder.RegisterInstance(new AutoRunService()).As<IAutoRun>();
            IoC.Builder.RegisterInstance(new NavigationService()).As<INavigationService>();
            IoC.Builder.RegisterInstance(new CredentialManager()).As<ICredentialManager>();
            IoC.Builder.RegisterInstance(new NewsFactory(new INewsService[]
            {
                new WINewsService(),
                new ZUTNewsService(),
            })).As<INewsFactory>();

            IoC.Compile();
        }

        private void LoadSettings()
        {
            XmlSerializer xs = new XmlSerializer(typeof(Storage));
            try
            {
                using (var sr = new StreamReader(new FileStream(Storage.SettingsFilePath,
                                                                FileMode.Open,
                                                                FileAccess.Read,
                                                                FileShare.ReadWrite)))
                {
                    IoC.Settings.CopyFrom(xs.Deserialize(sr) as Storage);
                }
            }
            catch (Exception e)
            {
                // Some error with settings file, do nothing
            }
        }



    }
}
