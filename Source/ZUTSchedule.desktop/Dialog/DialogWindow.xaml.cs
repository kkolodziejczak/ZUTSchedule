using System.Windows;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow(DialogViewModel viewModel)
        {
            InitializeComponent();

            // Set DataContext to ViewModel
            DataContext = viewModel;

            // Set Startup Position to center
            Owner = Application.Current.MainWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        /// <summary>
        /// Sets Dialog result to true
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButtonClick(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
