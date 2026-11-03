using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_Kardex")]
public class Kardex : IKardexProduct, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[NotMapped]
	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public short Ordinal { get; set; }

	public Guid Guid { get; set; }

	public Guid ProductGuid { get; set; }

	public Guid DepotGuid { get; set; }

	public DateTime DateM { get; set; }

	public Guid DetailGuid { get; set; }

	public Guid SanadGuid { get; set; }

	public Document SanadType { get; set; }

	public decimal Discount { get; set; }

	public decimal Fare { get; set; }

	public decimal VisitorPrice { get; set; }

	public decimal Tax { get; set; }

	public decimal VorodQT { get; set; }

	public decimal KhorojQT { get; set; }

	public decimal UnitPrice { get; set; }

	public decimal? RemQT { get; set; }

	public decimal? UnitSood { get; set; }

	public decimal? TotalSood { get; set; }

	public decimal? RowSood { get; set; }

	public decimal? validated { get; set; }

	public decimal? RemUnitPrice { get; set; }

	public decimal? RowSum { get; set; }

	public decimal? RemTotalPrice { get; set; }

	public Guid? UserGuid { get; set; }

	[Required]
	public string? Description { get; set; }

	[Required]
	public string? RDescription { get; set; }

	[Required]
	public string? ID2 { get; set; }

	public decimal? TotalHazine { get; set; }

	public string? Serial { get; set; }

	public decimal? Avarez { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? MoeinGuid { get; set; }

	public bool Invalidated { get; set; }

	public decimal AmaniCount { get; set; }

	public string? MetaData { get; set; }

	public Guid? V2Guid { get; set; }

	public decimal V2Qt { get; set; }

	public decimal DiscountPercent { get; set; }

	[MaxLength(50)]
	public string? DiscountPercentString { get; set; }

	public bool KardexAffective { get; set; }

	public bool IsTaxImpactOnCost { get; set; }

	public decimal CostPrice { get; set; }

	public decimal TotalCostPrice { get; set; }

	public Guid? ProductTafsilGuid { get; set; }

	public Cashier CashierItem { get; set; }

	public Product Product { get; set; }

	public Depot Depot { get; set; }

	public Moein Moein { get; set; }

	public Tafsil Tafsil { get; set; }

	public Tafsil ProductTafsil { get; set; }
}
