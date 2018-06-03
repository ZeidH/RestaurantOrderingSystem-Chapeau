using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChapeauModel;
using ChapeauDAL;
using System.Data;

namespace ChapeauLogic
{
    public class Processing_Service
    {
		Processing_DAO processingDao = new Processing_DAO();

		public DataTable GetReadyKitchenOrders()
		{
			return processingDao.Db_get_orders_by_status_and_location(OrderStatus.Ready, PreparationLocation.Kitchen);
		}
        

    }
}
