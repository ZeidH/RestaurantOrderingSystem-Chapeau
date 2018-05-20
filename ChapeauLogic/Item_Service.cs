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
        public Item NewOrderItem(int item_id, string item_comment, int order_id, float item_cost, int item_stock, MenuCategory menuCategory)
        {
            Item item = new Item
            {
                Order_id = order_id,
                Item_comment = item_comment,
                Order_time = DateTime.Now,
                Order_status = OrderStatus.Processing,
                Item_amount = 1,
                Item_cost = item_cost,
                Item_stock = item_stock-1,
                Item_category = menuCategory,
                Item_id = item_id
            };
            return item;
        }

        public float TotalCost(List<Item> items)
        {
            float total_cost = 0;
            foreach (Item item in items)
            {
                total_cost += item.Item_cost;
            }
            return total_cost;
        }

        public Item IncreaseAmount(Item item)
        {
            if (item.Item_stock <= 0)
            {
                throw new Exception("This item is out of stock!");
            }
            item.Item_amount++;
            item.Item_stock--;
            return item;
        }

        public Item DecreaseAmount(Item item)
        {
            if (item.Item_amount <= 1)
            {
                throw new Exception("This item is already at it's minimum amount!");
            }
            item.Item_amount--;
            item.Item_stock++;
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
