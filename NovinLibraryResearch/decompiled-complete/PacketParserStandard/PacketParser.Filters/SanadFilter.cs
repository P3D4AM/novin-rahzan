using System;
using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class SanadFilter
{
	public long NumberFrom { get; set; }

	public long NumberTo { get; set; }

	public List<int> NumberList { get; set; }

	public decimal SumFrom { get; set; }

	public decimal SumTo { get; set; }

	public decimal DebitFrom { get; set; }

	public decimal DebitTo { get; set; }

	public decimal CreditFrom { get; set; }

	public decimal CreditTo { get; set; }

	public Guid? CashierGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? MoeinGuid { get; set; }

	public DateTime? DateFrom { get; set; }

	public DateTime? DateTo { get; set; }

	public SanadSt? SanadStatus { get; set; }

	public string Description { get; set; } = "";


	public string RowDescription { get; set; } = "";


	public long TafsilCodeFrom { get; set; }

	public long TafsilCodeTo { get; set; }

	public long MoeinCodeFrom { get; set; }

	public long MoeinCodeTo { get; set; }

	public bool IsSelective { get; set; }

	public Document? SanadType { get; set; }

	public bool Status { get; set; } = true;


	public int? StatusId { get; set; }

	public List<SanadType> SanadTypeList { get; set; }
}
