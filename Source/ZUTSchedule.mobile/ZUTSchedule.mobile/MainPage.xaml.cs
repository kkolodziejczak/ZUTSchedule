using Simple.Xamarin.Framework.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ZUTSchedule.mobile
{
	public partial class MainPage : BaseContentPage
	{

		public MainPage()
		{
			InitializeComponent();

            BindingContext = new BasePageViewModel()
            {
                PageTitle = "Simple Test",
                LeftButtonTitle = "Left",
                RightButtonTitle = "Right",
            };
        }
	}
}
