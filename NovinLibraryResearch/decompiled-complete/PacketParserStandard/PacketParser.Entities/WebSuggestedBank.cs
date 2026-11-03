using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebSuggestedBank : ISuggestedBank, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string ShobeCode { get; set; }

	public string AccountNo { get; set; }

	public string BankName { get; set; }
}
