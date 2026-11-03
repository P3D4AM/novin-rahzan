using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.DepotReceipt;

namespace EntityCache.Business.Accounting.DepotReceipt;

public class DepotReceiptReportBusiness : IDepotReceipt, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public int Id2 { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => DateM.DateToFormatTime();

	public Guid? TafsilGuid { get; set; }

	public string TafsilName { get; set; }

	public string Description { get; set; }

	public bool IsTemp { get; set; }

	public bool KardexAffective => !IsTemp;

	public Guid CashierGuid { get; set; }

	public string CashierName { get; set; }

	public string CarNumber { get; set; }

	public string DriverName { get; set; }

	public string DriverPhone { get; set; }

	public string BarnameNo { get; set; }

	public string RefrencedNumber { get; set; }

	public EnDepotReceiptType Type { get; set; }

	public string TypeForDisplay => Type.GetDisplay().GetTranslate();

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid? OrderGuid { get; set; }
}
