using System;
using PacketParser;

namespace EntityCache.Business.Accounting;

public class DepEnteghalReportBusiness
{
	public Guid Guid { get; set; }

	public DateTime Date { get; set; }

	public string DateSh => Date.MiladiToCalture();

	public int Number { get; set; }

	public string Description { get; set; }

	public string ReceiverName { get; set; }

	public string DriverName { get; set; }

	public double SumQuantity { get; set; }

	public string SumQuantityForDisplay => SumQuantity.ToString().Replace(TextManager.Instance.DecimalPoint, "/");

	public decimal SumPrice { get; set; }
}
