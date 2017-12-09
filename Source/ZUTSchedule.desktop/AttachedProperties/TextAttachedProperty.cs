using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ZUTSchedule.desktop
{
    /// <summary>
    /// Focuses this element on load
    /// </summary>
    public class FocusOnLoadProperty :BaseAttachedProperty<FocusOnLoadProperty,bool>
    {
        public override void OnValueChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            if (!(sender is Control control))
                return;

            if ((bool)e.NewValue)
                control.Loaded += (s, se) => control.Focus();
        }
    }
}
