using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.Sanad;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.Sanad;

[Table("Acc_ViewSanadDetailCover")]
public class ViewSanadDetailCover : IViewSanadDetailCover, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public Guid? DebitMoeinGuid { get; set; }

	public Guid? DebitTafsilGuid { get; set; }

	public DateTime Date { get; set; }

	public Guid AsGuid { get; set; }

	public DocumentDet AsType { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public Guid? CashierGuid { get; set; }

	public int SanadNo { get; set; }

	public SanadSt SanadSt { get; set; }

	public int Id2 { get; set; }

	public Guid? CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public decimal Qt { get; set; }

	public decimal Price { get; set; }

	public decimal Qt2 { get; set; }

	public decimal RDiscount { get; set; }

	public decimal RBazaryab { get; set; }

	public string Serial { get; set; }

	public string SanadDesc { get; set; }

	public string RDesc { get; set; }

	public Guid? ProductGuid { get; set; }

	public Guid? DepotGuid { get; set; }

	public Guid? VahedGuid { get; set; }

	public DateTime? DateMoaser { get; set; }

	public Guid? DecimalGuid { get; set; }

	public decimal DecimalQt { get; set; }

	public decimal DecimalPrice { get; set; }

	public Guid? DetailGuid { get; set; }

	public DateTime Modified { get; set; }

	public Document AsTypeDocument { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public string? MetaData { get; set; }

	public Moein DebitMoeinItem { get; set; }

	public Tafsil DebitTafsilItem { get; set; }

	public Moein CreditMoeinItem { get; set; }

	public Tafsil CreditTafsilItem { get; set; }

	public Cashier CashierItem { get; set; }

	public DecimalEntity DecimalItem { get; set; }

	public Product ProductItem { get; set; }

	public Depot DepotItem { get; set; }

	public ProductVahed VahedItem { get; set; }

	public Tafsil Tafsil2 { get; set; }

	public Tafsil Tafsil3 { get; set; }

	public Tafsil Tafsil4 { get; set; }
}
