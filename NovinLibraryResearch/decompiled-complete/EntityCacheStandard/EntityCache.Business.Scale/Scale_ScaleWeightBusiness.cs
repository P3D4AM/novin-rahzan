using System;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Scale;

namespace EntityCache.Business.Scale;

public class Scale_ScaleWeightBusiness : IScale_ScaleWeight, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasContext
{
	public Guid ScaleGuid { get; set; }

	public Guid Guid { get; set; }

	public DateTime Date { get; set; }

	public decimal UnitPrice { get; set; }

	public string UnitPriceText { get; set; }

	public decimal Weight { get; set; }

	public string WeightText { get; set; }

	public decimal Totalprice { get; set; }

	public string TotalPriceText { get; set; }

	public string ContextGuid { get; set; }

	public DateTime Modified { get; set; }

	public bool Status { get; set; }

	public Scale_ScaleWeightBusiness(string contextGuid)
	{
		ContextGuid = contextGuid;
	}
}
