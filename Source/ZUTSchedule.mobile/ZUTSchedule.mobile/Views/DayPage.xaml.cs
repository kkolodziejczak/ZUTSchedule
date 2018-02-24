using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZUTSchedule.core;

namespace ZUTSchedule.mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DayPage : ContentPage, ISwipeCallBack
    {
        private Storage _settings;

		public DayPage ()
		{
			InitializeComponent();
            _settings = IoC.Settings;
            BindingContext = new WeekViewModel();
        }

        public void OnBottomSwipe(View view)
        {
            //throw new NotImplementedException();
        }

        public void OnLeftSwipe(View view)
        {
            _settings.DecrementWeek();
        }

        public void OnNothingSwiped(View view)
        {
            //throw new NotImplementedException();
        }

        public void OnRightSwipe(View view)
        {
            _settings.IncrementWeek();
        }

        public void OnTopSwipe(View view)
        {
            //throw new NotImplementedException();
        }
    }
}