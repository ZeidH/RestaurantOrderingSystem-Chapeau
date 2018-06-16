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
using ChapeauLogic;
using ChapeauModel;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Tableview_UI.xaml
    /// </summary>
    public partial class Tableview_UI : Page
    {
        public Tableview_UI(Employee employee)
        {
            InitializeComponent();
            GetTables();
        }
        public Tableview_UI()
        {
            InitializeComponent();
        }
        private void GetTables()
        {
            table_panel.Children.Add(new Table_UC(this));
        }
        internal void GenerateSidePanel(int order_id)
        {
            table_sidePanel.Children.Add(new TableSidePanel(order_id));
            this.AnimateIn();
        }
        internal void GenerateCreatePanel(int tableID)
        {
            
        }
        private async Task AnimateIn()
        {
            var sb = new Storyboard();
            var SlideAnimation = new ThicknessAnimation { Duration = new Duration(TimeSpan.FromSeconds(1.5)),
            From = new Thickness(-this.WindowWidth, 0, this.WindowWidth, 0),
            To = new Thickness(0),
            DecelerationRatio = 0.9f
            };
            Storyboard.SetTargetProperty(SlideAnimation, new PropertyPath("Margin"));
            Storyboard.SetTargetName(table_sidePanel, "SidePanel");
            sb.Children.Add(SlideAnimation);
            sb.Begin(table_sidePanel);

            await Task.Delay(2);
        }
    }
}
