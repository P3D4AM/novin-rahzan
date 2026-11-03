using System;
using System.ComponentModel;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace PacketParser.Entities;

public class WebKardex : IKardexProduct, IHasIsChecked, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public Guid DepotGuid { get; set; }

	public DateTime DateM { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent { get; set; }

	public string DiscountPercentString { get; set; }

	public string DiscountForDisplay { get; set; }

	public decimal Fare { get; set; }

	public decimal VisitorPrice { get; set; }

	public string VisitorPriceForDisplay { get; set; }

	public decimal Tax { get; set; }

	public decimal TaxForDisplay
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return Tax;
			}
			return 0m;
		}
	}

	public decimal TaxInpactOfCost
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return 0m;
			}
			return Tax;
		}
	}

	public Guid SanadGuid { get; set; }

	[Browsable(false)]
	public Document SanadType { get; set; }

	public Guid DetailGuid { get; set; }

	public decimal VorodQT { get; set; }

	public string VorodQtForDisplay { get; set; }

	public decimal KhorojQT { get; set; }

	public string KhorojQtForDisplay { get; set; }

	public decimal UnitPrice { get; set; }

	public string UnitPriceForDisplay { get; set; }

	public decimal? RemQT { get; set; }

	public string RemQtForDisplay { get; set; }

	public decimal? UnitSood { get; set; }

	public string UnitSoodForDisplay { get; set; }

	public decimal? TotalSood { get; set; }

	public string TotalSoodForDisplay { get; set; }

	public decimal? RowSood { get; set; }

	public string RowSoodForDisplay { get; set; }

	public decimal? RemUnitPrice { get; set; }

	public string RemUnitPriceForDisplay { get; set; }

	public decimal? RowSum { get; set; }

	public decimal? RowSumForDiplay
	{
		get
		{
			if (!IsTaxImpactOnCost)
			{
				return RowSum.GetValueOrDefault() + Tax;
			}
			return RowSum;
		}
	}

	public decimal? RemTotalPrice { get; set; }

	public string RemTotalPriceForDisplay { get; set; }

	public Guid? UserGuid { get; set; }

	public string Description { get; set; }

	public string DescForDisplay { get; set; }

	public string RDescription { get; set; }

	public string ID2 { get; set; }

	public decimal? TotalHazine { get; set; }

	public decimal TotalSumWithHazine => UnitPrice + TotalHazine.GetValueOrDefault();

	public string Serial { get; set; }

	public decimal? Avarez { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? MoeinGuid { get; set; }

	public string PrdName { get; set; }

	public string PrdCode { get; set; }

	public short Ordinal { get; set; }

	public string DepotName { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public bool IsChecked { get; set; }

	public string ContextGuid { get; set; }

	public decimal AmaniCount { get; set; }

	public string AmaniCountForDisplay => (AmaniCount + RemQT.GetValueOrDefault()).ToString().Replace(Text.DecimalPoint, "/").Replace("/000", "");

	public Guid? V2Guid { get; set; }

	public decimal V2Qt { get; set; }

	public string Vahed2Name { get; set; }

	public bool KardexAffective { get; set; }

	public bool SanadAffective { get; set; }

	public Guid ProductGuid { get; set; }

	public bool IsTaxImpactOnCost { get; set; }

	public decimal CostPrice { get; set; }

	public decimal TotalCostPrice { get; set; }

	public Guid? ProductTafsilGuid { get; set; }
}
