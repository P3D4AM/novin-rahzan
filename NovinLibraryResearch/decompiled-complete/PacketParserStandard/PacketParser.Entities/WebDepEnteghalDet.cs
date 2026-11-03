using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebDepEnteghalDet : IDepEnteghalDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid DptGuidSrc { get; set; }

	public Guid DptGuidDst { get; set; }

	public short Ordinal { get; set; }

	public Guid PrdGuid { get; set; }

	public decimal Quantity { get; set; }

	public Guid? VahedGuid { get; set; }

	public string Description { get; set; }

	public string Serial { get; set; }

	public decimal Price { get; set; }

	public decimal V2Qt { get; set; }

	public decimal V2Price { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;

}
