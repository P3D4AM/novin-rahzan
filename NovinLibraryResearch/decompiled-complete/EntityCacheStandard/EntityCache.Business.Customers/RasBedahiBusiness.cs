using System;

namespace EntityCache.Business.Customers;

public class RasBedahiBusiness
{
	public Guid CustomerGuid { get; set; }

	public long TafsilCode { get; set; }

	public string Fullname { get; set; }

	public Guid GroupGuid { get; set; }

	public decimal TafsilAccount { get; set; }

	public double TotalSood { get; set; }

	public string DateRas { get; set; }

	public decimal RasDays { get; set; }
}
