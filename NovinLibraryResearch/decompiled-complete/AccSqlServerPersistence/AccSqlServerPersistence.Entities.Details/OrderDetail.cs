using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DiscountCoupon;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;

namespace AccSqlServerPersistence.Entities.Details;

[Table("Acc_OrderDetail")]
public class OrderDetail : IOrderDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified
	{
		get
		{
			return _modified;
		}
		set
		{
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_modified = modified;
		}
	}

	[NotMapped]
	public bool Status { get; }

	public bool ChangeInQuantity { get; set; }

	public int Ordinal { get; set; }

	public Guid ProductGuid { get; set; }

	[Required]
	public string? ProductName { get; set; }

	public decimal Quantity { get; set; }

	public decimal Price { get; set; }

	public decimal? Price2 { get; set; }

	public Guid DepotGuid { get; set; }

	[Required]
	public string? Description { get; set; }

	public decimal Discount { get; set; }

	public Guid? VGuid { get; set; }

	public decimal? V2Qt { get; set; }

	public decimal V2Price { get; set; }

	public string? Serial { get; set; }

	public decimal VisitorPrice1 { get; set; }

	public decimal VisitorPrice2 { get; set; }

	public decimal VisitorPrice3 { get; set; }

	public decimal VisitorPrice4 { get; set; }

	public decimal TotalVisitorPrice { get; set; }

	public decimal TotalHazinePrice { get; set; }

	public decimal Tax { get; set; }

	public decimal Avarez { get; set; }

	public decimal RowSum { get; set; }

	public decimal Hazine1Price { get; set; }

	public decimal Hazine2Price { get; set; }

	public decimal Hazine3Price { get; set; }

	public decimal Hazine4Price { get; set; }

	public decimal Hazine5Price { get; set; }

	public Guid? DecimalGuid { get; set; }

	public decimal DecimalQt { get; set; }

	public decimal DecimalPrice { get; set; }

	public string? DecimalName { get; set; }

	public decimal DecimalSum { get; set; }

	public Guid? PriceTypeGuid { get; set; }

	public Guid MasterGuid { get; set; }

	public EntityChangeSt OrderDetState { get; set; }

	public string? SerialzeData { get; set; }

	public decimal TaxPercent { get; set; }

	public decimal AvarezPercent { get; set; }

	public decimal DiscountPercent { get; set; }

	[MaxLength(50)]
	public string? DiscountPercentString { get; set; }

	public Guid? DicountCouponDetailGuid { get; set; }

	public decimal FeePrice { get; set; }

	public decimal ProfitPrice { get; set; }

	public Guid? GrouppingGuid { get; set; }

	public Guid? ReferenceGuid { get; set; }

	public bool IsTaxImpactOnCost { get; set; }

	public Guid? ProductTafsilGuid { get; set; }

	public decimal CostPrice { get; set; }

	public decimal TotalCostPrice { get; set; }

	public string GiftDetailList { get; set; }

	[MaxLength(50)]
	public string SteppedDiscount { get; set; }

	[JsonIgnore]
	public Order OrderItem { get; set; }

	[JsonIgnore]
	public Product ProductItem { get; set; }

	[JsonIgnore]
	public Depot DepotItem { get; set; }

	[JsonIgnore]
	public ProductVahed VahedItem { get; set; }

	[JsonIgnore]
	public DecimalEntity DecimalItem { get; set; }

	[JsonIgnore]
	public ProductPriceLevel ProductPriceLevel { get; set; }

	[JsonIgnore]
	public Tafsil ProductTafsilItem { get; set; }

	[JsonIgnore]
	public OrderDetail Reference { get; set; }

	[JsonIgnore]
	public OrderDetail Group { get; set; }

	[JsonIgnore]
	public CouponDiscountDetail Coupon { get; set; }

	[JsonIgnore]
	public ICollection<OrderDetail> ReferenceDetail { get; set; }

	[JsonIgnore]
	public ICollection<OrderDetail> GrouppedDetail { get; set; }
}
