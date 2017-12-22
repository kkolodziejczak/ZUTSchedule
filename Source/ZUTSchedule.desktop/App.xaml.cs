using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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

            ApplicationSetup();

            Current.MainWindow = new MainWindow();
            Current.MainWindow.Show();
        }


        private void ApplicationSetup()
        {
            // setup IoC container 
            IoC.Setup();

            // Bind Navigation service
            IoC.Kernel.Bind<INavigationService>().ToConstant(new NavigationService());

            // Bind News factory
            IoC.Kernel.Bind<INewsFactory>().ToConstant(new NewsFactory(new INewsService[]
            {
                new WINewsService(),
                new ZUTNewsService(),
            }));

        }
    }
}
