using System;
using System.Collections.Generic;
using ChapeauModel;
using ChapeauDAL;
using System.Data.SqlClient;

namespace ChapeauLogic
{
    public class Item_Service : Table_Service
    {
        private Item_DAO itemDAO = new Item_DAO();
        public List<Item> GetMenu()
        {
            try
            {
                return itemDAO.DbSelectMenu();
            }
            catch (SqlException)
            {
                throw new Exception("The application could not connect to the database to receive the menu");
            }
            catch (Exception)
            {
                throw new Exception("There was a problem receiving the menu, please try again");
            }
        }

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

        #region CompleteOrder
        public void CompleteOrder(List<OrderItem> orderItems)
        {
            DateTime date = DateTime.Now;
            foreach (OrderItem item in orderItems)
            {
                AddFinalProperties(item, date);
            }
            //Send the orderlist to the DAO
            try
            {
                itemDAO.DbAddItem(orderItems);
            }
            catch (InvalidOperationException)
            {
                throw new Exception("A problem occured while sending the order to the database");
            }
            catch (SqlException)
            {
                throw new Exception("A problem occured with the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem has occured while completing the order");
            }
        }

        private void AddFinalProperties(OrderItem orderItem, DateTime date)
        {
            orderItem.Time = date;
            orderItem.Status = OrderStatus.Processing;
        }
        #endregion

        public bool CheckLunchTime()
        {
            int hour = DateTime.Now.Hour;
            return ((hour >= 11) && (hour <= 15));
        }

        public List<Item> RefreshStock(List<Item> menu)
        {
            try
            {
                List<int> newStock = itemDAO.DbRefreshStock();
                for (int i = 0; i < menu.Count; i++)
                {
                    menu[i].Stock = newStock[i];
                }
                return menu;
            }
            catch (SqlException)
            {
                throw new Exception("The application could not connect to the database to refresh the stock");
            }
            catch (Exception)
            {
                throw new Exception("The application could not refresh the stock");
            }
        }

        public bool VerifyStock(Item item)
        {
            try
            {
                return itemDAO.DbVerifyStock(item) > 0;
            }
            catch (SqlException)
            {
                throw new Exception("The application could not connect to the database to verify the stock");
            }
            catch (Exception)
            {
                throw new Exception("The application could not verify the stock");
            }
        }

        public void UpdateStock(OrderItem orderItem)
        {
            try
            {
                itemDAO.DbUpdateStock(orderItem);
            }
            catch (SqlException)
            {
                throw new Exception("The application could not connect to the database to update the stock");
            }
            catch (Exception)
            {
                throw new Exception("The application could not update the stock");
            }
        }

        public bool CheckAmount(int amount) => amount > 1;

        public bool CheckOrderCount(List<OrderItem> order) => order.Count > 0;

        #region IncreaseDecreaseLogic
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
        #endregion

        #region DeleteLogic
        public List<OrderItem> DeleteOrderItem(List<OrderItem> order, OrderItem orderItem)
        {
            orderItem.Item.Stock += orderItem.Amount;
            order.Remove(orderItem);
            return order;
        }

        public void DeleteOrderList(List<OrderItem> order)
        {
            foreach (OrderItem orderItem in order)
            {
                orderItem.Item.Stock += orderItem.Amount;
                UpdateStock(orderItem);
            }
        }
        #endregion

        public float GetTotalCost(List<OrderItem> items)
        {
            float totalCost = 0;
            foreach (OrderItem orderItem in items)
            {
                totalCost += orderItem.ReadTotalPrice;
            }
            return totalCost;
        }

        //Check if item has a meat type
        public bool CheckDinnerItem(Item item)
        {
            if (item.DinnerSubCategory == Dinner.Mains)
            {
                try
                {
                    return itemDAO.DbSelectMeatType(item.Item_id);
                }
                catch (SqlException)
                {
                    throw new Exception("The application could not connect to the database to check is the meat can be ordered rare, medium or well-done");
                }
                catch (Exception)
                {
                    throw new Exception("The application could not check wether selected item is a meat");
                }
            }
            return false;
        }
    }
}
