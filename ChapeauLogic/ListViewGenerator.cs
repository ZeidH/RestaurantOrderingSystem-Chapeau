using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.ServiceProcess;


namespace ChapeauLogic
{
    public class ListViewGenerator
    {
        protected ListView CreateTable(DataTable table)
        {
            ListView listView = new ListView();      

            // Create headers
            foreach (DataColumn column in table.Columns)
            {
                listView.Columns.Add(column.ColumnName);
            }

            // Fill data for each header
            foreach (DataRow row in table.Rows)
            {
                ListViewItem item = new ListViewItem(row[0].ToString());
                for (int i = 1; i < table.Columns.Count; i++)
                {
                    item.SubItems.Add(row[i].ToString());
                }
                listView.Items.Add(item);
            }
            return listView;
        }
    }
}
