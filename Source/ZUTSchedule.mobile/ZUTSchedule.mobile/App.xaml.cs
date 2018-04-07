using Simple.Xamarin.Framework;
using Simple.Xamarin.Framework.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

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

            var s = DisplayScreenWidth / DisplayXDpi;
            var value = GetDP(1);

            new SXF()
                .SetDefaultFontSize(s*5)
                .SetDefaultUnitSize(s*5)
                .Initialize();

            MainPage = new MainPage();
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
