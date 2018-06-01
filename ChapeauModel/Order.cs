using System.Collections.Generic;

namespace ChapeauModel
{
	public class Order
	{
		List<OrderItem> Items { get; set; }

		OrderStatus Status
		{
			get
			{
				// todo: combine order statuses of all the items
				return OrderStatus.Ready;
			}
		}
	}


	//public float Tip { get; set; }
	//public PayMethod PaymentMethod { get; set; }
}
