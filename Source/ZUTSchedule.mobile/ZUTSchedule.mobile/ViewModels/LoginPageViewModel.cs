using Simple.Xamarin.Framework.core;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ZUTSchedule.mobile.ViewModels
{
    public class LoginPageViewModel : BasePageViewModel
    {
        public string Text { get; set; }

        public LoginPageViewModel()
        {
            BottomToolBar.Show();
            Text = "Hello World! From MainPage.xaml";
            BottomToolBar.Show();
        }

        public override void InitializeNavigationBar()
        {
            NavigationBar = new NavigationBarViewModel
            {
                Title = "Sample Title",
                LeftButtonTitle = "Left",
                RightButtonTitle = "Right",
                LeftButtonCommand = new SequentialCommand(TestLeftButton),
                RightButtonCommand = new SequentialCommand(TestRightButton),
            };
        }

        public override void InitializeUpperToolBar()
        {
            UpperToolBar = new ToolBarViewModel()
                .AddItem("Test1", new SequentialCommand(() =>
                {
                    return Task.FromResult(2);
                }));
        }

        public override void InitializeBottomToolBar()
        {
        }

        public async Task TestLeftButton()
        {
            if (UpperToolBar.IsVisible)
                UpperToolBar.Hide();
            else
                UpperToolBar.Show();
        }

        public async Task TestRightButton(object param)
        {
            ShowProgressBar("Downloading...\nPlease Wait!");
            await Task.Delay(7 * 1000);
            ShowActivityIndicator("Please Wait!");
            HideProgressBar();
            await Task.Delay(5 * 1000);
            HideActivityIndicator();
        }
    }
}
