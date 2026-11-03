using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.Filters;

public class ReportGeneratorFilter
{
	public List<StiType> TypeList { get; set; }

	public bool? Status { get; set; } = true;


	public MaliYearSanadType? MaliYearSanadType { get; set; }
}
