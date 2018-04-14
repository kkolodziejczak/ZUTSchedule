using Autofac;
using Simple.Xamarin.Framework;

using Xamarin.Forms;
using ZUTSchedule.core;
using ZUTSchedule.mobile.Views;

namespace ZUTSchedule.mobile
{
    public partial class App : Application
	{
        public static double DisplayScreenWidth { get; set; }
        public static double DisplayScreenHeight { get; set; }
        public static double DisplayScaleFactor { get; set; }
        public static double DisplayXDpi { get; set; }

        public double GetDP(double valueInPX) => (valueInPX * DisplayXDpi) / 160;

        public App ()
		{
			InitializeComponent();

            new SXF()
                .SetDefaultFontSize(GetDP(4.5))
                .SetDefaultUnitSize(GetDP(3))
                .Initialize();

            ApplicationSetup();

            //MainPage = new MainPage();
            MainPage = new LoginPage();
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

        private void ApplicationSetup()
        {
            // setup IoC container
            IoC.Builder.RegisterInstance(new Storage()
            {
                HowManyDaysIsNew = 3,
                HowLongNewsMessages = 50,
                NumberOfDaysInTheWeek = 5,
            });
            IoC.Builder.RegisterInstance(new CommunicationService());
            IoC.Builder.RegisterInstance(new MessageService()).As<IMessageService>();
            IoC.Builder.RegisterInstance(new NavigationService()).As<INavigationService>();
            IoC.Builder.RegisterInstance(new CredentialManager()).As<ICredentialManager>();
            IoC.Compile();
        }

    }
}
