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
using ChapeauModel;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Orderview_MeatComments.xaml
    /// </summary>
    public partial class Orderview_MeatComments : UserControl
    {
        public Orderview_MeatComments()
        {
            InitializeComponent();
            CreateMeatCommmentButton();
        }

        private void CreateMeatCommmentButton()
        {
            int column_index = 0;
            foreach (CookingLevel level in Enum.GetValues(typeof(CookingLevel)))
            {
                CreateCookingLevelButtons(level.ToString(), column_index);
                column_index++;
            }
        }

        private void CreateCookingLevelButtons(string level, int column_index)
        {
            Button button = GetButton(level);
            Grid.SetColumn(button, column_index);
            Grid.SetRow(button, 0);
            main_grid.Children.Add(button);
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
            button.BeginAnimation(Button.OpacityProperty, animation);
        }

        private Button GetButton(string level)
        {
            return new Button
            {
                Content = level,
                Cursor = Cursors.Hand,
                Name = level,
                Background = new SolidColorBrush(Color.FromArgb(a: 255, r: 148, g: 148, b: 148)),
                Foreground = new SolidColorBrush(Color.FromArgb(a: 255, r: 255, g: 255, b: 255)),
                FontFamily = new FontFamily("Arial"),
                Opacity = 0,
                Width = 70
            };
        }
    }
}
