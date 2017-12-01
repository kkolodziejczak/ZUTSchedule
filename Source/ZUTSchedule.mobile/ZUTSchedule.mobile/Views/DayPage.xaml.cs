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
	public partial class DayPage : ContentPage
	{
		public DayPage ()
		{
			InitializeComponent ();
            var test = Storage.Classes.FirstOrDefault(d => d is DayViewModel);

            if(test != null)
            {
                BindingContext = test;
            }

		}
	}
}