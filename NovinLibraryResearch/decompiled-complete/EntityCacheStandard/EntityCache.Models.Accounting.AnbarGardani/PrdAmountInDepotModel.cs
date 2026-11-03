using System;

namespace EntityCache.Models.Accounting.AnbarGardani;

public class PrdAmountInDepotModel
{
	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public decimal RemQT { get; set; }

	public decimal RemUnitPrice { get; set; }
}
