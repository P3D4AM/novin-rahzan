using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TozieList;

namespace PacketParser.Entities;

public class WebTozieList : ITozieList, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public int Number { get; set; }

	public DateTime DateM { get; set; }

	public string DriverName { get; set; }

	public string DriverMobile1 { get; set; }

	public string DriverMobile2 { get; set; }

	public string CarNumber { get; set; }

	public string Description { get; set; }

	public Guid CashierGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Guid Guid { get; set; }
}
