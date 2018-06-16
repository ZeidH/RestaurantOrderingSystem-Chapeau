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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ChapeauLogic;
using ChapeauModel;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Login_UI.xaml
    /// </summary>
    public partial class Login_UI : Page
    {
        public Login_UI()
        {
            InitializeComponent();
            username_box.Focus();
            //lock_img.Source = new BitmapImage(new Uri("Images/LockIcon.png", UriKind.Relative));
            lock_img.Source = new BitmapImage(new Uri("Images/LockAnimation.gif", UriKind.Relative));
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            DirectUser(CheckUserInput());

        }
        private Employee CheckUserInput()
        {
            Employee employee = new Employee();
            try
            {
                Login_Service login_logic = new Login_Service(password_box.Password, username_box.Text);
                employee = login_logic.Validation();
            }
            catch (Exception ex)
            {
                password_box.Clear();
                password_box.Focus();
                MessageBox.Show(ex.Message, "Something wrong happend!", MessageBoxButton.OK, MessageBoxImage.Hand);
            }
            return employee;
        }
        private void DirectUser(Employee employee)
        {
            switch (employee.Occupation)
            {
                case Occupation.Waiter:
                    NavigationService.Navigate(new Tableview_UI(employee));
                    LockAnimation();
                    break;
                case Occupation.Kitchen:
                    NavigationService.Navigate(new Kitchenview_UI());
                    LockAnimation();
                    break;
                case Occupation.Bar:
                    NavigationService.Navigate(new Barview_UI());
                    LockAnimation();
                    break;
                //case Occupation.Manager:
                //    break;
                default:
                    break;
            }
        }

        private void LockAnimation()
        {
            //BitmapImage image = new BitmapImage(new Uri("Images/LockIcon.png", UriKind.Relative));
            ////lock_img.Source = new BitmapImage(new Uri("Images/LockAnimation.png", UriKind.Relative));
            //lock_img.DataContext = image;

            lock_img.Source = new BitmapImage(new Uri("Images/LockAnimation.gif", UriKind.Relative));
            //lock_img.Dispatcher.BeginInvoke(new Action(() => lock_img.Source = new BitmapImage(new Uri("Images/LockAnimation.png", UriKind.Relative))));
            System.Threading.Thread.Sleep(2000);
        }

        private void LoginPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                DirectUser(CheckUserInput());
            }
        }
    }
}
