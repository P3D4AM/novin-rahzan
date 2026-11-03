using System;
using PacketParser;

namespace EntityCache.Business.Accounting.Tolid.Summary;

public class TolidSummaryReportBusiness
{
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public int Id2 { get; set; }

	public string Description { get; set; }

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }
}
