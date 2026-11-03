using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IOrderDetail : IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	decimal? Price2 { get; set; }

	string Description { get; set; }

	decimal VisitorPrice1 { get; set; }

	decimal VisitorPrice2 { get; set; }

	decimal VisitorPrice3 { get; set; }

	decimal VisitorPrice4 { get; set; }

	decimal TotalVisitorPrice { get; }

	decimal TotalHazinePrice { get; set; }

	decimal Avarez { get; set; }

	decimal RowSum { get; }

	decimal V2Price { get; set; }

	decimal Hazine1Price { get; set; }

	decimal Hazine2Price { get; set; }

	decimal Hazine3Price { get; set; }

	decimal Hazine4Price { get; set; }

	decimal Hazine5Price { get; set; }

	Guid? DecimalGuid { get; set; }

	decimal DecimalQt { get; }

	decimal DecimalPrice { get; set; }

	decimal DecimalSum { get; }

	Guid? PriceTypeGuid { get; set; }

	EntityChangeSt OrderDetState { get; set; }

	int Ordinal { get; set; }

	decimal TaxPercent { get; set; }

	decimal AvarezPercent { get; set; }

	decimal DiscountPercent { get; }

	string DiscountPercentString { get; }

	Guid? DicountCouponDetailGuid { get; set; }

	Guid? ReferenceGuid { get; set; }

	decimal FeePrice { get; }

	decimal ProfitPrice { get; }

	Guid? GrouppingGuid { get; set; }

	bool IsTaxImpactOnCost { get; set; }

	Guid? ProductTafsilGuid { set; }

	decimal CostPrice { get; set; }

	decimal TotalCostPrice { get; set; }

	string SteppedDiscount { get; }
}
