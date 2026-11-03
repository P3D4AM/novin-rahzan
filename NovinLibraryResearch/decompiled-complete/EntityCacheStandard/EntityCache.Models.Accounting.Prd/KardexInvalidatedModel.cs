using System;

namespace EntityCache.Models.Accounting.Prd;

public class KardexInvalidatedModel
{
	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public string ContextGuid { get; set; }
}
