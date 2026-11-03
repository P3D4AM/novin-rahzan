using System;

namespace PacketParser.Entities.Interfaces.Accounting;

public interface IKardexProduct
{
	Guid Guid { get; set; }

	Guid DepotGuid { get; set; }

	DateTime DateM { get; set; }

	decimal Discount { get; set; }

	decimal Fare { get; set; }

	decimal VisitorPrice { get; set; }

	decimal Tax { get; set; }

	Guid SanadGuid { get; set; }

	Document SanadType { get; set; }

	Guid DetailGuid { get; set; }

	decimal VorodQT { get; set; }

	decimal KhorojQT { get; set; }

	decimal UnitPrice { get; set; }

	decimal? RemQT { get; set; }

	decimal? UnitSood { get; set; }

	decimal? TotalSood { get; set; }

	decimal? RowSood { get; set; }

	decimal? RemUnitPrice { get; set; }

	decimal? RowSum { get; set; }

	decimal? RemTotalPrice { get; set; }

	Guid? UserGuid { get; set; }

	string Description { get; set; }

	string RDescription { get; set; }

	string ID2 { get; set; }

	decimal? TotalHazine { get; set; }

	string Serial { get; set; }

	decimal? Avarez { get; set; }

	Guid? TafsilGuid { get; set; }

	Guid? MoeinGuid { get; set; }

	decimal AmaniCount { get; set; }

	decimal V2Qt { get; set; }

	decimal DiscountPercent { get; }

	string DiscountPercentString { get; }

	bool KardexAffective { get; set; }

	bool IsTaxImpactOnCost { get; set; }

	decimal CostPrice { get; set; }

	decimal TotalCostPrice { get; set; }

	Guid? ProductTafsilGuid { get; set; }
}
