using System.Windows;
using System.Windows.Controls;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();

            DataContext = new LoginViewModel();
        }

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if(DataContext is LoginViewModel viewModel)
            {
                viewModel.UserPassword = PasswordBox.SecurePassword;
            }
        }
    }
}
