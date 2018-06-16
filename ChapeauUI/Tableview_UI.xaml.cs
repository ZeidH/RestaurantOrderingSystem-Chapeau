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
            lbl_logged_user.Content = employee.Name;
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
        }
        internal void GenerateCreatePanel(int tableID)
        {
            
        }
    }
}
