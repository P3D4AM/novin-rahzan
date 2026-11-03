using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Factor;

namespace EntityCache.Business.Accounting.Factor;

public class SanadStatusLogBusiness : ISanadStatusLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public Guid? CashierGuid { get; set; }

	public string ContextGuid { get; set; }

	public int OldStatus { get; set; }

	public int NewStatus { get; set; }

	public DateTime DateM { get; set; }

	public int Ordinal { get; set; }

	public bool Status { get; set; }

	public SanadStatusLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
