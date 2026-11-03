using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebResid : IResid, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public long ID2 { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public DateTime Date1 { get; set; }

	public DateTime Date2 { get; set; }

	public Guid? GuidVahed { get; set; }

	public decimal Price { get; set; }

	public decimal Qt { get; set; }

	public decimal Mandeh { get; set; }

	public Guid? PrdGuid { get; set; }

	public string PrdModel { get; set; }

	public string PrdColor { get; set; }

	public string PrdSerial { get; set; }

	public Guid? TafsilGuid { get; set; }

	public string Problems { get; set; }

	public string Darkhast { get; set; }

	public short StatusId { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid? ReceptionGuid { get; set; }

	public Guid? OrderGuid { get; set; }

	public string Reserve { get; set; }
}
