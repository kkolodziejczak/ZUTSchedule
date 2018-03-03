using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ZUTSchedule.core;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Interaction logic for WeekView.xaml
    /// </summary>
    public partial class WeekView : UserControl
    {
        public WeekView()
        {
            InitializeComponent();

            try
            {
                DataContext = new WeekViewModel();
            }
            catch (HttpRequestException)
            {
                IoC.MessageService.ShowAlert("Connection issues");
            }
            catch (CredentialException)
            {
                IoC.MessageService.ShowAlert("Wrong Credentials");
            }
            catch (NoClassesException)
            {
                IoC.MessageService.ShowAlert("No classes to download");
                throw;
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            Animate();
        }

        public async Task Animate()
        {

            int time = 160;
            var w = Week.ActualWidth;
            var n = News.ActualWidth;

            var sb = new Storyboard();
            sb.AddSlideFromRight(time, w, n);

            // Start animating
            sb.Begin(this.News);

            // Make page visible
            this.News.Visibility = Visibility.Visible;

            // 40 sec == 3400px (news + window + (news - window)) [Width]
            // x  sec == 1705px (news)[Width]
            // 40*1705 == x * 3400
            // x = (40*1705) / 3400
            var dif = (n * time) / (n + w + (n - w));

            // Wait for it to finish
            await Task.Delay((int)(dif * 1000));

            var sb2 = new Storyboard();
            sb2.AddSlideFromRight(time, w, n);

            // Start animating
            sb2.Begin(this.News2);

            // Make page visible
            this.News2.Visibility = Visibility.Visible;
        }
    }

}
