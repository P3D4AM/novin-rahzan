using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebShortCut : IShortCut, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string FormName { get; set; }

	public string Name { get; set; }

	public int ShortCutKey { get; set; }

	public string Func { get; set; }

	public Guid CashierGuid { get; set; }

	public string Description { get; set; }

	public string BackColor { get; set; }

	public string ForeColor { get; set; }

	public string ImageSrc { get; set; }

	public bool Enable { get; set; }

	public bool Visable { get; set; }

	public short SubItems { get; set; }
}
