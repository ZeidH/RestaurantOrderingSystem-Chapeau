using System.Collections.Generic;

namespace ChapeauModel
{
	public class Order
	{
		public int Id { get; set; }

		public List<OrderItem> Items { get; set; }

		public int TableId { get; set; }

		public string EmployeeName { get; set; }
	}


	//public float Tip { get; set; }
	//public PayMethod PaymentMethod { get; set; }
}
