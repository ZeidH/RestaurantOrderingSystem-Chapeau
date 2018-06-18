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

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for OccupyTable.xaml
    /// </summary>
    public partial class OccupyTable : UserControl
    {
        private uint nrOfGuests = 1;
        private int emp_id;
        private int table_id;
        private Tableview_UI ParentPage;
        public OccupyTable(Tableview_UI ParentPage,int emp_id, int table_id)
        {
            InitializeComponent();
            this.ParentPage = ParentPage;
            this.emp_id = emp_id;
            this.table_id = table_id;
            btn_Decrement.IsEnabled = false;
            RefreshLabel();
        }

        private void Btn_Decrement_Click(object sender, RoutedEventArgs e)
        {
            nrOfGuests--;
            if (nrOfGuests <= 1)
                btn_Decrement.IsEnabled = false;
            btn_Increment.IsEnabled = true;
            RefreshLabel();
        }

        private void Btn_Increment_Click(object sender, RoutedEventArgs e)
        {
            nrOfGuests++;
            if (nrOfGuests >= 9)
                btn_Increment.IsEnabled = false;
            btn_Decrement.IsEnabled = true;
            RefreshLabel();
        }
        private void Btn_Occupy_Click(object sender, RoutedEventArgs e)
        {
            Table_Service logic = new Table_Service();
            logic.InsertOrder(emp_id, table_id, nrOfGuests);
            logic.SetTableStatus(ChapeauModel.TableStatus.Busy, table_id);
            ParentPage.CloseSidePanel();
        }
        private void RefreshLabel()
        {
            lbl_nrOfGuests.Content = nrOfGuests;
        }
    }
}
