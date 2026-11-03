using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_ProductBusiness : IScale_Product, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasDescription, IHasContext
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Code { get; set; }

	public Guid GroupGuid { get; set; }

	public Guid VahedGuid { get; set; }

	public bool IsWeight { get; set; }

	public decimal Price { get; set; }

	public string Barcode { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public string Description { get; set; }

	public string ContextGuid { get; set; }

	public Scale_ProductBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
