using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ChapeauModel;
/// <summary>
/// We need to stop making queries for ourselves and make them so that everyone can be able to use it. Get whole menu query instead of get part of menu query.
/// </summary>
namespace ChapeauDAL
{
    public class Item_DAO : Base
    {
        public void DbAddItem(List<OrderItem> orderItems)
        {
            SqlTransaction tran = OpenConnection().BeginTransaction();
            string query = string.Format("INSERT INTO ORDER_LIST (order_id, item_comment, order_time, order_status, item_amount, item_id) " +
            "VALUES(@orderid, @itemcomment, @ordertime, @orderstatus, @itemamount, @itemid)");
            try
            {
                foreach (OrderItem orderItem in orderItems)
                {
                    SqlParameter[] sqlParameter = new SqlParameter[6];
                    sqlParameter[0] = new SqlParameter("@orderid", SqlDbType.Int)
                    {
                        Value = orderItem.Item.Order_id
                    };
                    sqlParameter[1] = new SqlParameter("@itemcomment", SqlDbType.NVarChar)
                    {
                        Value = orderItem.Comment
                    };
                    sqlParameter[2] = new SqlParameter("@ordertime", SqlDbType.DateTime)
                    {
                        Value = orderItem.Time
                    };
                    sqlParameter[3] = new SqlParameter("@orderstatus", SqlDbType.SmallInt)
                    {
                        Value = orderItem.Status
                    };
                    sqlParameter[4] = new SqlParameter("@itemamount", SqlDbType.Int)
                    {
                        Value = orderItem.Amount
                    };
                    sqlParameter[5] = new SqlParameter("@itemid", SqlDbType.Int)
                    {
                        Value = orderItem.Item.Item_id
                    };
                    ExecuteEditTranQuery(query, sqlParameter, tran);
                }
                tran.Commit();
            }
            catch (SqlException e)
            {
                try
                {
                    tran.Rollback();
                }
                catch (Exception)
                {
                    ErrorFilePrint print = new ErrorFilePrint();
                    print.ErrorLog(e);
                    throw new Exception("The application has send an incomplete order, please contact the manager");
                }
                ErrorFilePrint print2 = new ErrorFilePrint();
                print2.ErrorLog(e);
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception e)
            {
                try
                {
                    tran.Rollback();
                }
                catch (Exception)
                {
                    ErrorFilePrint print = new ErrorFilePrint();
                    print.ErrorLog(e);
                    throw new Exception("The application has send an incomplete order, please contact the manager");
                }
                ErrorFilePrint print2 = new ErrorFilePrint();
                print2.ErrorLog(e);
                throw new Exception("A problem with the database has occured");
            }
        }

        public int DbVerifyStock(Item item)
        {
            string query = string.Format("SELECT item_stock FROM ITEM where item_id = @itemid");
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@itemid", SqlDbType.Int)
            {
                Value = item.Item_id
            };
            try
            {
                return VerifyStock(ExecuteSelectQuery(query, sqlParameters));
            }
            catch (SqlException)
            {
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }

        private int VerifyStock(DataTable dataTable)
        {
            try
            {
                return (int)dataTable.Rows[0]["item_stock"];
            }
            catch (SqlException)
            {
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }

        public List<int> DbRefreshStock()
        {
            string query = "SELECT item_stock FROM ITEM";
            SqlParameter[] sqlParameters = new SqlParameter[0];
            try
            {
                return RefreshStock(ExecuteSelectQuery(query, sqlParameters));
            }
            catch (SqlException)
            {
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }

        private List<int> RefreshStock(DataTable dataTable)
        {
            List<int> stocks = new List<int>();
            try
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    int stock = (int)dr["item_stock"];
                    stocks.Add(stock);
                }
                return stocks;
            }
            catch (SqlException)
            {
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }

        public void DbUpdateStock(OrderItem orderItem)
        {
            string query = string.Format("UPDATE ITEM SET item_stock = @itemstock WHERE item_id = @itemid");
            SqlParameter[] sqlParameters = new SqlParameter[2];
            sqlParameters[0] = new SqlParameter("@itemstock", SqlDbType.Int)
            {
                Value = orderItem.Item.Stock
            };
            sqlParameters[1] = new SqlParameter("@itemid", SqlDbType.Int)
            {
                Value = orderItem.Item.Item_id
            };
            try
            {
                ExecuteEditQuery(query, sqlParameters);
            }
            catch (SqlException)
            {
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }

        public List<Item> DbSelectMenu()
        {
            string query = "SELECT i.item_id, i.item_name, i.item_cost, i.item_stock, d.drink_category, l.lunch_category, di.dinner_category, d.drink_vat " +
                           "FROM((ITEM as i left JOIN drink as d on i.item_id = d.drink_id) left join LUNCH as l on i.item_id = l.lunch_id) left join dinner as di on i.item_id = di.dinner_id";
            SqlParameter[] sqlParameters = new SqlParameter[0];
           try
            {
                return ReadMenu(ExecuteSelectQuery(query, sqlParameters));
            }
            catch (SqlException)
            {
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }
        private List<Item> ReadMenu(DataTable dataTable)
        {
            List<Item> menu = new List<Item>();
            try
            {
                foreach (DataRow dr in dataTable.Rows)
                {
                    Item item = new Item()
                    {
                        Item_id = (int)dr["item_id"],
                        Name = dr["item_name"].ToString(),
                        Cost = (float)(double)dr["item_cost"],
                        Stock = (int)dr["item_stock"],
                    };
                    if (!dr.IsNull("drink_category"))
                    {
                        item.Category = MenuCategory.Drink;
                        item.DrinkSubCategory = (Drink)Int16.Parse(dr["drink_category"].ToString());
                        item.Vat = (Vat)Int16.Parse(dr["drink_vat"].ToString());
                    }
                    if (!dr.IsNull("lunch_category"))
                    {
                        item.Category = MenuCategory.Lunch;
                        item.LunchSubCategory = (Lunch)Int16.Parse(dr["lunch_category"].ToString());
                    }
                    if (!dr.IsNull("dinner_category"))
                    {
                        item.Category = MenuCategory.Dinner;
                        item.DinnerSubCategory = (Dinner)Int16.Parse(dr["dinner_category"].ToString());
                    }
                    menu.Add(item);
                }
                return menu;
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }

        public bool DbSelectMeatType(int item_id)
        {
            ErrorFilePrint print = new ErrorFilePrint();
            string query = "SELECT has_meat_type FROM dinner where dinner_id = @itemid";
            SqlParameter[] sqlParameters = new SqlParameter[1];
            sqlParameters[0] = new SqlParameter("@itemid", SqlDbType.Int)
            {
                Value = item_id
            };
            try
            {
                return ReadMeatType(ExecuteSelectQuery(query, sqlParameters));
            }
            catch (SqlException e)
            {
                print.ErrorLog(e);
                throw new Exception("The application was unable to connect to the database");
            }
            catch (Exception e)
            {
                print.ErrorLog(e);
                throw new Exception("A problem with the database has occured");
            }
        }

        private bool ReadMeatType(DataTable dataTable)
        {
            try
            {
                return (bool)dataTable.Rows[0]["has_meat_type"];
            }
            catch (Exception)
            {
                throw new Exception("A problem with the database has occured");
            }
        }
    }
}
