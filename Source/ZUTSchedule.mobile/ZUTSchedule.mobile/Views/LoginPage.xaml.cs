using Simple.Xamarin.Framework;
using System;
using System.Security;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ZUTSchedule.mobile.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : BaseContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            BindingContext = new LoginPageViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is LoginPageViewModel viewModel)
            {
                var newPassword = new SecureString();
                var str = e.NewTextValue;
                foreach (var c in e.NewTextValue)
                {
                    newPassword.AppendChar(c);
                }

                viewModel.LoginVM.UserPassword = newPassword;
            }
        }
    }
}