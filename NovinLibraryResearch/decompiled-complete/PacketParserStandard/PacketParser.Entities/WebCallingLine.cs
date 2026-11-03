using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Crm;

namespace PacketParser.Entities;

public class WebCallingLine : Serializable<WebCallingLine>, ICallingLine, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string ComputerName { get; set; }

	public short DeviceNumber { get; set; }

	public short LineNumber { get; set; }

	public string Number { get; set; }

	public bool IsCo { get; set; }

	public Guid CashierGuid { get; set; }

	public bool Status { get; set; }
}
