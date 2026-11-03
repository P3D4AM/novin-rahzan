using System;

namespace PacketParser.Entities.Interfaces.Scale;

public interface IScale_ScaleWeight : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	Guid ScaleGuid { get; set; }

	DateTime Date { get; set; }

	decimal UnitPrice { get; set; }

	string UnitPriceText { get; set; }

	decimal Weight { get; set; }

	string WeightText { get; set; }

	decimal Totalprice { get; set; }

	string TotalPriceText { get; set; }
}
