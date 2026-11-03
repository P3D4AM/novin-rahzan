using System;
using PacketParser;

namespace EntityCache.Business.Accounting.Hotel.ReportsT;

public class ReceiptReportBusiness
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public int Number { get; set; }

	public Guid TafsilGuid { get; set; }

	public string CustomerName { get; set; }

	public string Description { get; set; }

	public string ProsenterName { get; set; }

	public int PeopleCount { get; set; }

	public decimal TotalPrice { get; set; }

	public string UserName { get; set; }

	public decimal Discount { get; set; }

	public bool IsChecked { get; set; }
}
