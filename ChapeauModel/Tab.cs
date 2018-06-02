using System.Collections.Generic;

namespace ChapeauModel
{
	public class Tab
	{
		List<OrderItem> Items { get; set; }

		float TotalPrice
		{
			get
			{
				float total = 0f;

				foreach (OrderItem item in Items)
				{
					total += item.TotalPrice;
				}

				return total;
			}
		}
	}


	//public float Tip { get; set; }
	//public PayMethod PaymentMethod { get; set; }
}
