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
    /// Interaction logic for Orderview_sub_category.xaml
    /// </summary>
    public partial class Orderview_sub_category : UserControl
    {
        public Orderview_sub_category(string sub_category)
        {
            InitializeComponent();
            CheckAmountSubcateogry(sub_category);
            CreateSubCategoryButtons(sub_category);
        }
        ////Size the buttons inside the stackpanel
        //private void SizeStackPanelChildren()
        //{
        //    double buttonWidth = StackPanel_sub_category.Width / StackPanel_sub_category.Children.Count;
        //    foreach (Button button in StackPanel_sub_category.Children)
        //    {
        //        button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
        //        button.Width = buttonWidth - 4;
        //    }
        //}

        private int CheckAmountSubcateogry(string sub)
        {
            //Either check to which enum the sub_category belongs, or check to which category the category belongs
            //After get all the sub categories and put them into the wrappanel
            int amount = 0;
            return amount;
        }

        //Create buttons, add them to stackpanel and assign .Click event handler
        private void CreateSubCategoryButtons(string sub_category)
        {
            Button button = new Button
            {
                Content = sub_category,
                Cursor = Cursors.Hand,
                Name = sub_category,
                Background = new SolidColorBrush(Color.FromArgb(a: 255, r: 199, g: 203, b: 207)),
                FontFamily = new FontFamily("Arial"),
                Opacity = 0,
            };
            //button.Click += new RoutedEventHandler(ButtonSubCategory_Click);
            Grid.SetColumn(button, 0);
            Grid.SetRow(button, 0);
            main_grid.Children.Add(button);
            DoubleAnimation animation = new DoubleAnimation(1, TimeSpan.FromMilliseconds(600));
            button.BeginAnimation(Button.OpacityProperty, animation);
        }
    }
}
