using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Bill;

namespace EntityCache.Business.Accounting.Bill;

public class BillMasterBusiness : IBillMaster, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public string ContextGuid { get; set; }

	public Guid Guid { get; set; }

	public Guid CashierGuid { get; set; }

	public Guid? PardakhtGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public SanadSt SanadSt { get; set; }

	public int SanadNumber { get; set; }

	public DateTime Date { get; set; }

	public decimal Sum { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public BillMasterBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
