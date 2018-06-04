using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using ChapeauModel;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Orderview_MeatComments.xaml
    /// </summary>
    public partial class Orderview_MeatComments : UserControl
    {
        public string meat_comment;
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
            Style style = Application.Current.FindResource("MeatComments") as Style;
            Button button = new Button()
            {
                Style = style,
                Content = level,
                Name = level,
                Opacity = 0,
                Width = 70
            };
            button.Click += new RoutedEventHandler(ButtonMeatComment_Click);
            return button;
        }

        private void ButtonMeatComment_Click(object sender, RoutedEventArgs e)
        {
            RevertClicked();
            Button button = (sender as Button);
            button.Style = Application.Current.FindResource("ClickedMeatComments") as Style;
            string comment = (sender as Button).Content.ToString();
            switch (comment)
            {
                case "R":
                    meat_comment = "Rare";
                    break;
                case "M":
                    meat_comment = "Medium";
                    break;
                case "WD":
                    meat_comment = "Well-done";
                    break;
                default:
                    break;
            }
        }

        public void RevertClicked()
        {
            foreach (Button button in main_grid.Children)
            {
                button.Style = Application.Current.FindResource("MeatComments") as Style;
            }
        }
    }
}
