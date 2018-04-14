using Simple.Xamarin.Framework;

namespace ZUTSchedule.mobile
{
    public partial class MainPage : BaseContentPage
	{
		public MainPage()
		{
			InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }
    }
}
