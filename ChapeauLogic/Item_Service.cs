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
        private Item_DAO item_DAO = new Item_DAO();
        public List<Item> ReadMenu()
        {
            return item_DAO.Db_select_meu();
        }

        public List<Item> GetSubMenu(List<Item> menu, string subCategory)
        {
            string[] splitted = subCategory.Split(' ');
            List<Item> subMenu = new List<Item>();
            for (int i = 0; i < menu.Count; i++)
            {
                if (splitted[1] == menu[i].LunchSubCategory.ToString())
                {
                    subMenu.Add(menu[i]);
                }
                else if (splitted[1] == menu[i].DinnerSubCategory.ToString())
                {
                    subMenu.Add(menu[i]);
                }
                else if (splitted[1] == menu[i].DrinkSubCategory.ToString())
                {
                    subMenu.Add(menu[i]);
                }
            }
            return subMenu;
        }

        public void CompleteOrder(List<OrderItem> orderItems)
        {
            DateTime date = DateTime.Now;
            foreach (OrderItem item in orderItems)
            {
                AddFinalProperties(item, date);
            }
            item_DAO.Db_add_item(orderItems);
        }

        public bool CheckLunchTime()
        {
            int hour = DateTime.Now.Hour;
            return ((hour >= 11) && (hour <= 15));
        }

        public List<Item> RefreshStock(List<Item> menu)
        {
            List<int> newStock = item_DAO.Db_refresh_stock();
            for (int i = 0; i < menu.Count; i++)
            {
                menu[i].Stock = newStock[i];
            }
            return menu;
        }

        public bool VerifyStock(Item item)
        {
            return item_DAO.Db_verify_stock(item) > 0;
        }

        public void UpdateStock(OrderItem orderItem)
        {
            item_DAO.Db_update_stock(orderItem);
        }

        private void AddFinalProperties(OrderItem orderItem, DateTime date)
        {
            orderItem.Time = date;
            orderItem.Status = OrderStatus.Processing;
            if (orderItem.Comment == null)
            {
                orderItem.Comment = "";
            }
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

        public void IncreaseAmount(OrderItem orderItem)
        {
            if (orderItem.Item.Stock <= 0)
            {
                throw new Exception("This item is out of stock!");
            }
            orderItem.Amount++;
            orderItem.Item.Stock--;
        }

        public void DecreaseAmount(OrderItem orderItem)
        {
            if (orderItem.Amount <= 1)
            {
                throw new Exception("This item is already at it's minimum amount!");
            }
            orderItem.Amount--;
            orderItem.Item.Stock++;
        }

        public List<OrderItem> DeleteOrderItem(List<OrderItem> order, OrderItem orderItem)
        {
            orderItem.Item.Stock += orderItem.Amount;
            order.Remove(orderItem);
            return order;
        }

        public bool CheckStock(int stock)
        {
            return stock > 0;
        }

        public bool CheckAmount(int amount)
        {
            return amount > 1;
        }

        public bool CheckOrderCount(List<OrderItem> order)
        {
            return order.Count > 0;
        }

        public List<bool> CheckOrderStock(List<Item> subMenuItems)
        {
            List<bool> op = new List<bool>();
            for (int i = 0; i < subMenuItems.Count; i++)
            {
                if (subMenuItems[i].Stock > 0)
                {
                    op.Add(true);
                    continue;
                }
                op.Add(false);
            }
            return op;
        }

        //remove
        public float GetTotalCost(List<OrderItem> items)
        {
            float total_cost = 0;
            foreach (OrderItem orderItem in items)
            {
                //Maybe use payment? vat?
                total_cost += orderItem.Item.Cost * orderItem.Amount;
            }
            return total_cost;
        }

        public bool CheckDinnerItem(Item item)
        {
            if (item.DinnerSubCategory == Dinner.Mains)
            {
                return item_DAO.Db_select_meat_type(item.Item_id);
            }
            return false;
        }
    }
}
