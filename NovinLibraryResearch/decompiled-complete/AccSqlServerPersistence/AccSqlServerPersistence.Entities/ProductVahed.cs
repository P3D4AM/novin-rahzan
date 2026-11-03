using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using AccSqlServerPersistence.Entities.Accounting.Tolid;
using AccSqlServerPersistence.Entities.Details;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using SharedAccEntityFramworkPersistence.Entities.Accounting.DepotReceiptClass;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Products;
using SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_ProductVahed")]
public class ProductVahed : IPrdVahed, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
{
	[Required]
	public string Description { get; set; }

	[Required]
	public string Name { get; set; }

	public bool Status { get; set; }

	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string? DeliveryMessage { get; set; }

	public string? DeliveryShortMessage { get; set; }

	public string? HostRefrenced { get; set; }

	public bool DecimalSales { get; set; }

	public Guid? TaxUnitGuid { get; set; }

	public ICollection<DepEnteghalDet> DepEnteghalDets { get; set; }

	public ICollection<OrderDetail> OrderDetailItem { get; set; }

	public ICollection<GardeshTafsil> GardeshTafsils { get; set; }

	public ICollection<TolidExport> TolidExport { get; set; }

	public ICollection<TolidFormulaExport> TolidFormulaExport { get; set; }

	public ICollection<TolidFormulaMavad> TolidFormulaMavad { get; set; }

	public ICollection<TolidMavad> TolidMavad { get; set; }

	public ICollection<Product> Products { get; set; }

	public ICollection<ViewSanadDetailCover> ViewSanadDetailCover { get; set; }

	public ICollection<DepotReceiptDetail> DepotReceiptDetail { get; set; }

	public ICollection<ProductPriceRelation> ProductPriceRelation { get; set; }
}
