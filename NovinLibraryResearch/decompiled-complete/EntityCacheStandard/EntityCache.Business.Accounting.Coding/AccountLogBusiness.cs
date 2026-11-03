using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Coding;

namespace EntityCache.Business.Accounting.Coding;

public class AccountLogBusiness : IAccountLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public DateTime Date { get; set; }

	public decimal NewAccount { get; set; }

	public decimal OldAccount { get; set; }

	public string DebuggerInfo { get; set; }

	public ENAction Action { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	public bool Status { get; set; } = true;


	public string ActionForDisplay => Action.GetDisplay().GetTranslate();

	public string ContextGuid { get; set; }

	public AccountLogBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
