using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebRasGiriDet : IRasGiriDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public decimal Price { get; set; }

	public decimal Enheraf { get; set; }

	public string Description { get; set; }

	public DateTime DateM { get; set; }
}
