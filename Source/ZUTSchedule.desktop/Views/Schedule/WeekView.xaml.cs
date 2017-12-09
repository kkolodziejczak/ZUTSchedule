using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
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

            DataContext = new WeekViewModel();
        }

        private async void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            await Animate();
        }

        public async Task Animate()
        {

            int time = 40;
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
            var dif = (n * time) / (n + w + (n-w));

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
