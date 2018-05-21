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
                Comment = item_comment,
                Time = DateTime.Now.ToString("yyyyMMddHHmmss"),
                Status = OrderStatus.Processing,
                Amount = 1,
                Cost = item_cost,
                Stock = item_stock-1,
                Category = menuCategory,
                Item_id = item_id
            };
            return item;
        }

        public float TotalCost(List<Item> items)
        {
            float total_cost = 0;
            foreach (Item item in items)
            {
                total_cost += item.Cost;
            }
            return total_cost;
        }

        public void IncreaseAmount(Item item)
        {
            if (item.Stock <= 0)
            {
                throw new Exception("This item is out of stock!");
            }
            item.Amount++;
            item.Stock--;
        }

        public void DecreaseAmount(Item item)
        {
            if (item.Amount <= 1)
            {
                throw new Exception("This item is already at it's minimum amount!");
            }
            item.Amount--;
            item.Stock++;
        }

        public List<Item> DeleteOrderItem(List<Item> items, int item_id)
        {
            items.RemoveAt(item_id);
            return items;
        }

        private Item_DAO item_DAO = new Item_DAO();
        public void AddItem(Item item)
        {
            item_DAO.Db_add_item(item);
        }

        public DataTable GetItems(int order_id)
        {
            DataTable dataTable = item_DAO.Db_select_items(order_id);
            return dataTable;
        }

        public DataTable GetStatus(int order_id)
        {
            DataTable dataTable = item_DAO.Db_select_status(order_id);
            return dataTable;
        }

        public DataTable GetMenu(MenuCategory menu, int category)
        {
            DataTable dataTable = item_DAO.Db_select_menu_items(menu, category);
            return dataTable;
        }

        public int FindCategory(string source, MenuCategory menu)
        {
            string[] splitted = source.Split(' ');
            int sub_category = 0;
            switch (splitted[1])
            {
                case "Beers":
                    sub_category = (int)Drink.Beers;
                    break;
                case "HotDrinks":
                    sub_category = (int)Drink.HotDrinks;
                    break;
                case "SoftDrinks":
                    sub_category = (int)Drink.SoftDrinks;
                    break;
                case "Wines":
                    sub_category = (int)Drink.Wines;
                    break;
                case "Desserts":
                    sub_category = (int)Dinner.Desserts;
                    break;
                case "Mains":
                    sub_category = (int)Dinner.Mains;
                    break;
                case "Starters":
                    sub_category = (int)Dinner.Starters;
                    break;
                case "Bites":
                    sub_category = (int)Lunch.Bites;
                    break;
                case "Main":
                    sub_category = (int)Lunch.Main;
                    break;
                case "Specials":
                    sub_category = (int)Lunch.Specials;
                    break;
            }
            return sub_category;
        }
    }
}
