using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ChapeauUI
{
    public static class Animation
    {
        internal static void AnimateIn(Page page, int duration)
        {
            var sb = new Storyboard();
            var fadeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(duration)),
                From = 0,
                To = 1
            };
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTargetName(page, "page");
            sb.Children.Add(fadeAnimation);
            sb.Begin(page);
        }
        internal static void AnimateOut(Page page)
        {
            var sb = new Storyboard();
            var fadeAnimation = new DoubleAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                From = 1,
                To = 0
            };
            Storyboard.SetTargetProperty(fadeAnimation, new PropertyPath("Opacity"));
            Storyboard.SetTargetName(page, "page");
            sb.Children.Add(fadeAnimation);
            sb.Begin(page);
        }
        internal static void AnimateSlide(WrapPanel panel, double left, double right )
        {
            var sb = new Storyboard();
            var SlideAnimation = new ThicknessAnimation
            {
                Duration = new Duration(TimeSpan.FromSeconds(1.5)),
                From = new Thickness(-left, 0, right, 0),
                To = new Thickness(0),
                DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(SlideAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetName(panel, "SidePanel");
            sb.Children.Add(SlideAnimation);
            sb.Begin(panel);
        }
    }
}
