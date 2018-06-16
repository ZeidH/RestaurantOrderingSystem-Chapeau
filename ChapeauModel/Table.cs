using System;

namespace ChapeauModel
{

	public class Tafel
	{
        public int ID { get; set; }
		public TableStatus Status { get; set; }
        public Employee Employee { get; set; }
        public int NumberOfGuests { get; set; }
        public DateTime WaitingTime { get; set; }
        public int OrderID { get; set; }

	}


	//public float Tip { get; set; }
	//public PayMethod PaymentMethod { get; set; }
}
