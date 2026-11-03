using System;
using System.Collections.Generic;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebPrdSelected : IPrdSelected, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string Code { get; set; }

	public string Title { get; set; }

	public short? Type { get; set; }

	public Guid UserGuid { get; set; }

	public string Description { get; set; }

	public List<WebPrdSelectedDetList> DetList { get; set; }
}
