using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MvvmProject.View.AttachedProperties
{
    /// Attached property to control the button 
    /// enable or diable based on logged in user type
    public class DesiredSize : FrameworkElement
    {
        public static double GetDesiredWidth(DependencyObject obj)
        {
            return (double)obj.GetValue(DesiredWidthProperty);
        }

        public static double GetDesiredHeight(DependencyObject obj)
        {
            return (double)obj.GetValue(DesiredHeightProperty);
        }

        public static void SetDesiredWidth(DependencyObject obj, double value)
        {
            obj.SetValue(DesiredWidthProperty, value);
        }

        public static void SetDesiredHeight(DependencyObject obj, double value)
        {
            obj.SetValue(DesiredHeightProperty, value);
        }

        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.RegisterAttached("DesiredWidth",
            typeof(double), typeof(DesiredSize), new PropertyMetadata(Double.NaN));

        public static readonly DependencyProperty DesiredHeightProperty =
            DependencyProperty.RegisterAttached("DesiredHeight",
            typeof(double), typeof(DesiredSize), new PropertyMetadata(Double.NaN));

    }
}
