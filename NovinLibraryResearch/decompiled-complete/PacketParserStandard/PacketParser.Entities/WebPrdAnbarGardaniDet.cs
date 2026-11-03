using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebPrdAnbarGardaniDet : IPrdAnbarGardaniDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid MasterGuid { get; set; }

	public Guid PrdGuid { get; set; }

	public Guid DptGuid { get; set; }

	public decimal QtKasr { get; set; }

	public decimal QtEzafe { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal KasrSum { get; }

	public decimal EzafeSum { get; }

	public decimal? QtRealAmount { get; set; }

	public List<WebPrdSerial> SerialStr { get; set; }
}
