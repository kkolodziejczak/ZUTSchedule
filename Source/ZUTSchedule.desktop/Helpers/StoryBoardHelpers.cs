using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;

namespace ZUTSchedule.desktop
{
    public static class StoryBoardHelpers
    {
        public static void AddSlideFromRight(this Storyboard storyboard, float seconds, double windwWidth, double newsWidth)
        {
            double endPoint = (newsWidth + (newsWidth - windwWidth));


            // Create the margin animate from right 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(windwWidth, 0, -windwWidth, 0),
                To = new Thickness(-endPoint, 0, endPoint, 0),
                RepeatBehavior = RepeatBehavior.Forever,
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }

        public static void AddSlideToLeft(this Storyboard storyboard, float seconds, double windowOffset, double Textoffset)
        {

            // Create the margin animate from right 
            var animation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(seconds)),
                From = new Thickness(0),
                To = new Thickness(-(Textoffset-windowOffset), 0, 0, 0),
                RepeatBehavior = RepeatBehavior.Forever,
            };

            // Set the target property name
            Storyboard.SetTargetProperty(animation, new PropertyPath("Margin"));

            // Add this to the storyboard
            storyboard.Children.Add(animation);
        }
    }
}
