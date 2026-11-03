using System;

namespace EntityCache.Business.Accounting.Coding.ReportsT;

public class HesabGroupReportBusiness
{
	public Guid Guid { get; set; }

	public int Code { get; set; }

	public string Name { get; set; }

	public string TypeName { get; set; }

	public bool Status { get; set; }
}
