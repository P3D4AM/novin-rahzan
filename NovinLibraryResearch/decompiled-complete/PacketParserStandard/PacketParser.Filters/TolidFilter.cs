using System;
using System.Collections.Generic;

namespace PacketParser.Filters;

public class TolidFilter
{
	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public bool? KardexAffective { get; set; }

	public bool? SanadAffective { get; set; }

	public Guid? CashierGuid { get; set; }

	public int NumberFrom { get; set; }

	public int NumberTo { get; set; }

	public List<int> NumberList { get; set; }

	public string Description { get; set; } = "";

}
