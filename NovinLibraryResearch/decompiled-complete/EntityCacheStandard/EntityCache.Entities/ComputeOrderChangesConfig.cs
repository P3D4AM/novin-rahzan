using System.Collections.Generic;
using EntityCache.Business;
using PacketParser.BusinessLayer;
using PacketParser.Entities;

namespace EntityCache.Entities;

public class ComputeOrderChangesConfig
{
	public string ContextGuid { get; private set; }

	public OrderDetailBusiness OrderDetial { get; set; }

	public Document FactorType { get; set; }

	public List<ProductsChanges> PrdChanges { get; set; }

	public bool IsNew { get; set; }

	public bool IsDeleteAction { get; set; }

	public EnSystem System { get; set; }

	public ComputeOrderChangesConfig(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
