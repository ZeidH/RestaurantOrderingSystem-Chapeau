﻿using System;
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
using System.Collections.ObjectModel;
using ChapeauModel;
using ChapeauLogic;

namespace ChapeauUI
{
    /// <summary>
    /// Interaction logic for Table_UC.xaml
    /// </summary>
    public partial class Table_UC : UserControl
    {
        private ObservableCollection<Tafel> tables = new ObservableCollection<Tafel>();
        private Tableview_UI table_main;
        public Table_UC(Tableview_UI table_main)
        {
            InitializeComponent();
            this.table_main = table_main;
            GetTableInfo();
            InsertTableInfo();
        }

        private void GetTableInfo()
        {
            Table_Service table_Logic = new Table_Service();
            tables = table_Logic.FillTables();
        }

        private void InsertTableInfo()
        {
            Label[] nrOfGuest = new Label[10];
            nrOfGuest[0] = lbl_NrOfGuests_1;
            nrOfGuest[1] = lbl_NrOfGuests_2;
            nrOfGuest[2] = lbl_NrOfGuests_3;
            nrOfGuest[3] = lbl_NrOfGuests_4;
            nrOfGuest[4] = lbl_NrOfGuests_5;
            nrOfGuest[5] = lbl_NrOfGuests_6;
            nrOfGuest[6] = lbl_NrOfGuests_7;
            nrOfGuest[7] = lbl_NrOfGuests_8;
            nrOfGuest[8] = lbl_NrOfGuests_9;
            nrOfGuest[9] = lbl_NrOfGuests_10;
            Label[] status = new Label[10];
            status[0] = lbl_Status_1;
            status[1] = lbl_Status_2;
            status[2] = lbl_Status_3;
            status[3] = lbl_Status_4;
            status[4] = lbl_Status_5;
            status[5] = lbl_Status_6;
            status[6] = lbl_Status_7;
            status[7] = lbl_Status_8;
            status[8] = lbl_Status_9;
            status[9] = lbl_Status_10;
            Label[] waiter = new Label[10];
            waiter[0] = lbl_Waiter_1;
            waiter[1] = lbl_Waiter_2;
            waiter[2] = lbl_Waiter_3;
            waiter[3] = lbl_Waiter_4;
            waiter[4] = lbl_Waiter_5;
            waiter[5] = lbl_Waiter_6;
            waiter[6] = lbl_Waiter_7;
            waiter[7] = lbl_Waiter_8;
            waiter[8] = lbl_Waiter_9;
            waiter[9] = lbl_Waiter_10;

            for (int i = 0; i < waiter.Length; i++)
            {
                if (tables[i].Status != TableStatus.Free)
                {
                    nrOfGuest[i].Content = tables[i].NumberOfGuests.ToString("P0");
                    waiter[i].Content = tables[i].Employee.Name;
                    status[i].Content = tables[i].Status.ToString();
                }
                else
                {
                    status[i].Content = TableStatus.Free.ToString();
                }
            }
        }


        private void Btn_Table_Click(object sender, RoutedEventArgs e)
        {
            CheckTable(Splitter((sender as Button).Name.ToString()));
        }

        private void CheckTable(int tableID)
        {
            if (tables[tableID].Status == TableStatus.Free)
            {
                table_main.GenerateCreatePanel(tableID);
            }
            else
            {
                table_main.GenerateSidePanel(tables[tableID].OrderID);
            }
        }

        // Splits the label/button names to get which row
        private int Splitter(string value)
        {
            string[] splitted = value.Split('_');
            return int.Parse(splitted[2]);
        }
    }
}