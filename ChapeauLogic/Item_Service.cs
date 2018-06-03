using System;
using System.Collections.Generic;
using ChapeauModel;
using ChapeauDAL;

namespace ChapeauLogic
{
    public class Item_Service
    {
        private Item_DAO item_DAO = new Item_DAO();
        public List<Item> ReadMenu() => item_DAO.Db_select_meu();

        public List<Item> GetSubMenu(List<Item> menu, string subCategory)
        {
            string[] splitted = subCategory.Split(' ');
            List<Item> subMenu = new List<Item>();
            foreach (Item item in menu)
            {
                if (splitted[1] == item.LunchSubCategory.ToString())
                {
                    subMenu.Add(item);
                }
                else if (splitted[1] == item.DinnerSubCategory.ToString())
                {
                    subMenu.Add(item);
                }
                else if (splitted[1] == item.DrinkSubCategory.ToString())
                {
                    subMenu.Add(item);
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

        public bool VerifyStock(Item item) => item_DAO.Db_verify_stock(item) > 0;

        public void UpdateStock(OrderItem orderItem) => item_DAO.Db_update_stock(orderItem);

        private void AddFinalProperties(OrderItem orderItem, DateTime date)
        {
            orderItem.Time = date;
            orderItem.Status = OrderStatus.Processing;
            if (orderItem.Comment == null)
            {
                orderItem.Comment = "";
            }
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

        public bool CheckStock(int stock) => stock > 0;

        public bool CheckAmount(int amount) => amount > 1;

        public bool CheckOrderCount(List<OrderItem> order) => order.Count > 0;

        //remove? ask 
        public float GetTotalCost(List<OrderItem> items)
        {
            float total_cost = 0;
            foreach (OrderItem orderItem in items)
            {
                //payment stuff
                total_cost += orderItem.Item.Cost * orderItem.Amount;
            }
            return total_cost;
        }

        //Check if item is a meat 
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
