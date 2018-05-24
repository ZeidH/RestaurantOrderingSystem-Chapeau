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
        public void CompleteOrder(List<OrderItem> orderItems)
        {
            DateTime date = DateTime.Now;
            foreach (OrderItem item in orderItems)
            {
                AddFinalProperties(item, date);
            }
            item_DAO.Db_add_item(orderItems);
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
            //calculate in gridview
            orderItem.Amount++;
            orderItem.Item.Stock--;
        }

        public void DecreaseAmount(OrderItem orderItem)
        {
            if (orderItem.Amount <= 1)
            {
                throw new Exception("This item is already at it's minimum amount!");
            }
            //calculate in gridview
            orderItem.Amount--;
            orderItem.Item.Stock++;
        }

        public List<OrderItem> DeleteOrderItem(List<OrderItem> order, OrderItem orderItem)
        {
            order.Remove(orderItem);
            return order;
        }

        //public DataTable GetItems(int order_id)
        //{
        //    DataTable dataTable = item_DAO.Db_select_items(order_id);
        //    return dataTable;
        //}

        //public DataTable GetStatus(int order_id)
        //{
        //    DataTable dataTable = item_DAO.Db_select_status(order_id);
        //    return dataTable;
        //}

        public List<Item> ReadMenu()
        {
            List<Item> menu = item_DAO.Db_select_meu();
            return menu;
        }

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

        public List<Item> GetSubMenu(List<Item> menu, string subCategory)
        {
            List<Item> subMenu = new List<Item>();
            for (int i = 0; i < menu.Count; i++)
            {
                if (Enum.IsDefined(typeof(Lunch), subCategory) && menu[i].LunchSubCategory != null)
                {
                    subMenu.Add(menu[i]);
                }
                else if (Enum.IsDefined(typeof(Dinner), subCategory) && menu[i].DinnerSubCategory != null)
                {
                    subMenu.Add(menu[i]);
                }
                else if (Enum.IsDefined(typeof(Drink), subCategory) && menu[i].DrinkSubCategory != null)
                {
                    subMenu.Add(menu[i]);
                }
            }
            return subMenu;
        }

        public bool CheckStock(Item item)
        {
            return item.Stock > 0;
        }

        public bool CheckAmount(int amount)
        {
            return amount > 1;
        }

        public bool CheckOrderCount(List<OrderItem> order)
        {
            return order.Count > 0;
        }

        //public int FindCategory(string source, MenuCategory menu)
        //{
        //    string[] splitted = source.Split(' ');
        //    int sub_category = 0;
        //    switch (splitted[1])
        //    {
        //        case "Beers":
        //            sub_category = (int)Drink.Beers;
        //            break;
        //        case "HotDrinks":
        //            sub_category = (int)Drink.HotDrinks;
        //            break;
        //        case "SoftDrinks":
        //            sub_category = (int)Drink.SoftDrinks;
        //            break;
        //        case "Wines":
        //            sub_category = (int)Drink.Wines;
        //            break;
        //        case "Desserts":
        //            sub_category = (int)Dinner.Desserts;
        //            break;
        //        case "Mains":
        //            sub_category = (int)Dinner.Mains;
        //            break;
        //        case "Starters":
        //            sub_category = (int)Dinner.Starters;
        //            break;
        //        case "Bites":
        //            sub_category = (int)Lunch.Bites;
        //            break;
        //        case "Main":
        //            sub_category = (int)Lunch.Main;
        //            break;
        //        case "Specials":
        //            sub_category = (int)Lunch.Specials;
        //            break;
        //    }
        //    return sub_category;
        //}
    }
}
