using System;
using PacketParser;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Hotel;

namespace EntityCache.Business.Accounting.Hotel;

public class RoomKardexBusiness : IRoomKardex, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid Guid { get; set; }

	public Guid AsGuid { get; set; }

	public Guid RoomGuid { get; set; }

	public Guid CashiarGuid { get; set; }

	public int OldStatus { get; set; }

	public int NewStatus { get; set; }

	public int Ordinal { get; set; }

	public string Discription { get; set; }

	public DateTime DateM { get; set; }

	public DateTime Modified { get; set; }

	public EnAsTypy AsType { get; set; }

	public string AsTypeForDisplay => AsType.GetDisplay();

	public bool Status { get; set; } = true;


	public string ContextGuid { get; set; }

	public RoomKardexBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
