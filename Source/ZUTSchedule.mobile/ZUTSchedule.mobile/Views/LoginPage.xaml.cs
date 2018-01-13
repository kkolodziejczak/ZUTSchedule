using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ZUTSchedule.core;

namespace ZUTSchedule.mobile
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent ();
            BindingContext = new LoginViewModel(0);
        }

        private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (BindingContext is LoginViewModel viewModel)
            {
                viewModel.UserPassword = new SecureString();
                foreach (var character in PasswordBox.Text)
                {
                    viewModel.UserPassword.AppendChar(character);
                }
            }
        }
    }
}