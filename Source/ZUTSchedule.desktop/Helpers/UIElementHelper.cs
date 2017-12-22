using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace ZUTSchedule.desktop
{
    public static class UIElementHelper
    {
        public static void DragOnClick(this UIElement element, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DependencyObject parent = element;
                int avoidInfiniteLoop = 0;
                // Search up the visual tree to find the first parent window.
                while ((parent is Window) == false)
                {
                    parent = VisualTreeHelper.GetParent(parent);
                    avoidInfiniteLoop++;
                    if (avoidInfiniteLoop == 1000)
                    {
                        // Something is wrong - we could not find the parent window.
                        return;
                    }
                }
                var window = parent as Window;
                window.DragMove();
            }
        }
    }
}
