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
using System.Windows.Threading;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Redirect.xaml
    /// </summary>
    public partial class Redirect : Page
    {
        DispatcherTimer dispatcherTimer = new DispatcherTimer();

        public Redirect(string message)
        {

            InitializeComponent();
            Animation.AnimateIn(redirect_page, 1);
            lblMessage.Content = message;
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
            dispatcherTimer.Start();
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            NavigationService.Navigate(new Tableview_UI());
        }
    }
}
