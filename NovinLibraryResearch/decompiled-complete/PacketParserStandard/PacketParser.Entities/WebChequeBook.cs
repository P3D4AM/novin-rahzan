using System;
using System.ComponentModel;
using Entities.Interfaces.Accounting.Cheque;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebChequeBook : IChequeBook, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string SerialNumber { get; set; }

	public Guid BankGuid { get; set; }

	public bool IsSaderShode { get; set; }
}
