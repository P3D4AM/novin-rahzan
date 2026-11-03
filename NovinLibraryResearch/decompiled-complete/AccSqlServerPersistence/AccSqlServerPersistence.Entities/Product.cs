using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Selection;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Entities.Details;
using AccSqlServerPersistence.Entities.Global;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Hotel;
using SharedAccEntityFramworkPersistence.Entities.Accounting.OrderClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Tax;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_Product")]
public class Product : IProduct, IProductCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[StringLength(100)]
	public bool AutoSellSt { get; set; }

	public decimal BPercent1 { get; set; }

	public decimal BPercent2 { get; set; }

	public decimal BPercent3 { get; set; }

	public decimal BPercent4 { get; set; }

	public long Code { get; set; }

	[NotMapped]
	public bool IsDisposed { get; private set; }

	public DateTime CreateDate { get; set; }

	[Required]
	public string? Description { get; set; }

	public decimal DiscountPercent { get; set; }

	public string? FullContent { get; set; }

	public decimal Height { get; set; }

	[Required]
	[MaxLength(200)]
	public string? IdProduct { get; set; }

	public bool IsTax { get; set; }

	public string? Tags { get; set; }

	public EnProductType Type { get; set; }

	public bool IsSerial { get; set; }

	public decimal KardexPrice { get; set; }

	public string? Keywords { get; set; }

	public string? MetaDesc { get; set; }

	public ProductMojodiSt Mojodi { get; set; }

	[Required]
	public string? Name { get; set; }

	public string? OtherPics { get; set; }

	[Required]
	public string? Pic { get; set; }

	public string? QuickText { get; set; }

	public bool ScaleSt { get; set; }

	public decimal Score { get; set; }

	public bool Special { get; set; }

	public bool Status { get; set; }

	public decimal V2Qt { get; set; }

	public decimal V3Qt { get; set; }

	public decimal V4Qt { get; set; }

	[NotMapped]
	public string? VahedName { get; }

	public PayementType PayementType { get; set; }

	[MaxLength(100)]
	[Required]
	public string? Sku { get; set; }

	[MaxLength(100)]
	[Required]
	public string? Slug { get; set; }

	public decimal Width { get; set; }

	public decimal Weight { get; set; }

	public Guid GuidVahed { get; set; }

	public Guid? V2Guid { get; set; }

	public Guid? V3Guid { get; set; }

	public Guid? V4Guid { get; set; }

	[Required]
	public string? ImageList { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public decimal OrderPoint { get; set; }

	public Guid? MasterGuid { get; set; }

	public Guid? TolidFormulaGuid { get; set; }

	public int Version { get; set; }

	public DateTime? SendToServerDate { get; set; }

	public decimal Length { get; set; }

	public EnProductIndex IndexingType { get; set; }

	public EnProductKind Kind { get; set; }

	public bool IsDependentOnCurrency { get; set; }

	public Guid? TaxGuid { get; set; }

	public DateTime? DiscountStartDate { get; set; }

	public DateTime? DiscountEndDate { get; set; }

	public decimal DiscountPrice { get; set; }

	public string? DecimalMetaData { get; set; }

	[MaxLength(500)]
	public string? MappId { get; set; }

	[Required]
	public string? NameForDisplay { get; set; }

	public Guid? DefaultVariable { get; set; }

	public virtual ICollection<ProductAttributeRelation> ProductAttributeRelations { get; set; }

	public virtual ICollection<DepEnteghalDet> DepEnteghalDets { get; set; }

	public virtual ICollection<PrdAnbarRelation> PrdAnbarRelations { get; set; }

	public virtual ICollection<OrderDetail> OrderDetails { get; set; }

	public virtual ICollection<Resid> Resid { get; set; }

	public virtual ICollection<PrdTechnical> Technicals { get; set; }

	public virtual ICollection<Barcode> Barcodes { get; set; }

	public virtual ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public virtual ICollection<Kardex> Kardexes { get; set; }

	public virtual ICollection<TolidExport> TolidExport { get; set; }

	public virtual ICollection<TolidFormulaExport> TolidFormulaExport { get; set; }

	public virtual ICollection<TolidFormulaMavad> TolidFormulaMavad { get; set; }

	public virtual ICollection<PrdAnbarGardaniDet> AnbarGardani { get; set; }

	public virtual ICollection<TolidMavad> TolidMavad { get; set; }

	public virtual ICollection<TolidSummaryDetail> TolidSummaryDetail { get; set; }

	public virtual ICollection<ViewSanadDetailCover> ViewSanadDetailCover { get; set; }

	public virtual TolidFormula TolidFormulaItem { get; set; }

	public virtual ICollection<ProductPriceUpdate> Updates { get; set; }

	public virtual ICollection<LastPrice> LastPrice { get; set; }

	public virtual ICollection<PrdSerial> Serials { get; set; }

	public virtual ICollection<ProductDecimalRelation> ProductPrices { get; set; }

	public virtual ICollection<ProductGroupRelation> ProductGroupRelations { get; set; }

	public virtual ICollection<ServiceReceiptDetailRelation> ServiceRecieptDetail { get; set; }

	public virtual ICollection<OrderDetailCouponCalculation> OrderDetailCouponCalculation { get; set; }

	public virtual ICollection<DepotReceiptDetail> DepotReceiptDetail { get; set; }

	public ICollection<TempBarcodePrint> TempBarcodePrint { get; set; }

	public ICollection<ProductPriceRelation> ProductPriceRelation { get; set; }

	public ProductVahed VahedItem { get; set; }

	public TaxClass TaxItem { get; set; }
}
