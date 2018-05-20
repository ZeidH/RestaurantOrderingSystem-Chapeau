using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;
using System.Configuration;
using System.Data;

namespace ChapeauLogic
{
    public class Item_Service
    {
        //readstatus
        //add items
        //get items

        public Item NewOrderItem(int item_id, string item_comment, int order_id)
        {
            Item item = new Item
            {
                Order_id = order_id,
                Item_comment = item_comment,
                Order_time = DateTime.Now,
                Order_status = OrderStatus.Processing,
                Item_amount = 1,
                Item_id = item_id,
            };
            return item;
        }

        public Item IncreaseAmount(Item item)
        {
            item.Item_amount++;
            return item;
        }

        public Item DecreaseAmount(Item item)
        {
            item.Item_amount--;
            return item;
        }

        public List<Item> DeleteOrderItem(List<Item> items, int item_id)
        {
            items.RemoveAt(item_id);
            return items;
        }

        public void AddItem(Item item)
        {
            Item_DAO item_DAO = new Item_DAO();
            item_DAO.Db_add_item(item);
        }

        public DataTable GetItems(int order_id)
        {
            Item_DAO item_DAO = new Item_DAO();
            DataTable dataTable = item_DAO.Db_select_items(order_id);
            return dataTable;
        }

        public DataTable GetStatus(int order_id)
        {
            Item_DAO item_DAO = new Item_DAO();
            DataTable dataTable = item_DAO.Db_select_status(order_id);
            return dataTable;
        }

        public DataTable GetMenu(MenuCategory menu, int category)
        {
            Item_DAO item_DAO = new Item_DAO();
            DataTable dataTable = item_DAO.Db_select_menu_items(menu, category);
            return dataTable;
        }
    }
}
