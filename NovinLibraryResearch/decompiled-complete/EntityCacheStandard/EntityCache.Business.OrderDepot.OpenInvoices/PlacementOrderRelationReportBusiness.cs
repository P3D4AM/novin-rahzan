using System;
using EntityCache.Utilities;
using PacketParser;

namespace EntityCache.Business.OrderDepot.OpenInvoices;

public class PlacementOrderRelationReportBusiness
{
	public int Id2 { get; set; }

	public int DailyOrderNumber { get; set; }

	public string Id2ForDisplay => $"{Id2} ({DailyOrderNumber})";

	public Guid OrderGuid { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => $"{DateM.Hour}:{DateM.Minute}  {DateM.MiladiToCalture()}";

	public string TafsilName { get; set; }

	public decimal Discount { get; set; }

	public decimal Tax { get; set; }

	public decimal TotalSum { get; set; }

	public string PlacementTitle { get; set; }

	public Guid PlacementGuid { get; set; }

	public byte[] EditImage => ImageResourceManager.OpenInvoiceEdit;

	public byte[] SettlementImage => ImageResourceManager.OpenInvoiceSettlement;
}
