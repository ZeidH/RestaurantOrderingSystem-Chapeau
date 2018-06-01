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
using System.Data;
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
        }

        private void Login_button_Click(object sender, RoutedEventArgs e)
        {
            Login lgn = new Login();

            lgn.username = Convert.ToString(username_txtbox);
            lgn.pwdhash = Convert.ToString(password_txtbox);

            Login_Service lgnService = new Login_Service(); //create an istance of the class to make it work as non-static field

            lgnService.GetCredentials(lgn);  //pass the input from the user to the logic layer

            

            Employee employee = new Employee();
            
            switch (employee.occupation)
            {
                case Occupation.Waiter:
                    NavigationService.Navigate(new Tableview_UI());
                    break;
                case Occupation.Bar:
                    NavigationService.Navigate(new Barview_UI());
                    break;

                case Occupation.Kitchen:
                    NavigationService.Navigate(new Kitchenview_UI());
                    break;
               // case Occupation.Manager:
                   // NavigationService.Navigate(new Tableview_UI());
                  //  break;
            }
        }
    }
}
