using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebPrdTechnical : IPrdTechnical, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string Code { get; set; }

	public string Name { get; set; }

	public string Key { get; set; }

	public short TechnicalType { get; set; }

	public Guid ProductGuid { get; set; }
}
