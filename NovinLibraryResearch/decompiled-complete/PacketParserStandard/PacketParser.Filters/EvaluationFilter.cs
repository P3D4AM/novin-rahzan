using System;
using System.Collections.Generic;

namespace PacketParser.Filters;

public class EvaluationFilter
{
	public DateTime? StartDate { get; set; }

	public DateTime? EndDate { get; set; }

	public Guid? CashierGuid { get; set; }

	public int? Id2From { get; set; }

	public int? Id2To { get; set; }

	public List<int> NumberList { get; set; }

	public int? SanadNoFrom { get; set; }

	public int? SanadNoTo { get; set; }

	public List<int> SanadNoList { get; set; }

	public string Description { get; set; }
}
