using System;
using System.ComponentModel;
using Entities.Interfaces;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebCusCard : ICusCard, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public Guid CusGuid { get; set; }

	public string CardName { get; set; }

	public string CardNo { get; set; }

	[Obsolete("", true)]
	public string STDesc { get; set; }

	[Obsolete("", true)]
	public string STName { get; set; }

	[Obsolete("", true)]
	public string CusCode { get; set; }

	[Obsolete("", true)]
	public string CusName { get; set; }

	public string Pass { get; set; }

	public DateTime DateStart { get; set; }

	public DateTime DateExpire { get; set; }

	public string Description { get; set; }
}
