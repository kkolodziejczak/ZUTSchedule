using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZUTSchedule.core;

using Xamarin.Forms;
using Autofac;

namespace ZUTSchedule.mobile
{


    public partial class App : Application
	{
        public static IContainer Container { get; set; }

        public static Page Page { get; private set; }

		public App ()
		{
            ApplicationSetup();

            InitializeComponent();

            MainPage = new NavigationPage( new LoginPage());
            
            MainPage.ToolbarItems.Add(new ToolbarItem()
            {
                Text = "R",
                Order = ToolbarItemOrder.Secondary,
            });
            MainPage.ToolbarItems.Add(new ToolbarItem()
            {
                Text = "L",
                Order = ToolbarItemOrder.Secondary,
            });
            MainPage.ToolbarItems.Add(new ToolbarItem()
            {
                Text = "C",
                Order = ToolbarItemOrder.Primary,
            });

            Page = this.MainPage;
        }

        private void ApplicationSetup()
        {
            IoC.Builder.RegisterInstance(new NavigationService()).As<INavigationService>();
            IoC.Builder.RegisterInstance(new NewsFactory()).As<INewsFactory>();
            IoC.Setup();
        }

        protected override void OnStart ()
		{
            // Handle when your app starts
        }

        protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
