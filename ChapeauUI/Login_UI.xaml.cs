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
using System.Windows.Media.Animation;
using ChapeauLogic;
using ChapeauModel;
using WpfAnimatedGif;
using System.Windows.Threading;

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
            lock_img.Source = new BitmapImage(new Uri("Images/LockIcon.png", UriKind.Relative));
        }

        private void Btn_Login_Click(object sender, RoutedEventArgs e)
        {
            Employee employee = CheckUserInput();
            if (employee.Occupation == null)
            {
                return;
            }
            else
            {
                DirectUser(employee);
            }
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
            LockAnimation();
            var timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Start();
            timer.Tick += (sender, args) =>
            {
                timer.Stop();
                switch (employee.Occupation)
                {
                    case Occupation.Waiter:
                        NavigationService.Navigate(new Tableview_UI(employee));
                        break;
                    case Occupation.Kitchen:
                        NavigationService.Navigate(new Kitchenview_UI());
                        break;
                    case Occupation.Bar:
                        NavigationService.Navigate(new Barview_UI());
                        break;
                    //case Occupation.Manager:
                    //    break;
                    default:
                        break;
                }
            };
        }

        private void LockAnimation()
        {
            var image = new BitmapImage();
            image.BeginInit();
            image.UriSource = new Uri("Images/LockAnimation.gif", UriKind.Relative);
            image.EndInit();
            ImageBehavior.SetAnimatedSource(lock_img, image);
            
            ImageBehavior.SetAutoStart(lock_img, true);
            ImageBehavior.SetRepeatBehavior(lock_img, RepeatBehavior.Forever);
        }

        private void LoginPage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Employee employee = CheckUserInput();
                if (employee.Occupation == null)
                {
                    return;
                }
                else
                {
                    DirectUser(employee);
                }
            }
        }
    }
}
