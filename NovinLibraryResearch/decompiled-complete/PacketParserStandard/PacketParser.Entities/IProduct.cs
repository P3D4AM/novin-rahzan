using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public interface IProduct : IProductCompact, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	bool AutoSellSt { get; set; }

	decimal BPercent1 { get; set; }

	decimal BPercent2 { get; set; }

	decimal BPercent3 { get; set; }

	decimal BPercent4 { get; set; }

	DateTime CreateDate { get; set; }

	string FullContent { get; set; }

	decimal Height { get; set; }

	bool IsSerial { get; set; }

	decimal KardexPrice { get; set; }

	string Keywords { get; set; }

	string MetaDesc { get; set; }

	ProductMojodiSt Mojodi { get; set; }

	string OtherPics { get; set; }

	string QuickText { get; set; }

	bool ScaleSt { get; set; }

	decimal Score { get; set; }

	decimal V3Qt { get; set; }

	decimal V4Qt { get; set; }

	decimal Width { get; set; }

	decimal Weight { get; set; }

	Guid? V3Guid { get; set; }

	Guid? V4Guid { get; set; }

	Guid? MasterGuid { get; set; }

	PayementType PayementType { get; set; }

	string Sku { get; set; }

	string Slug { get; set; }

	Guid? TolidFormulaGuid { get; set; }

	int Version { get; set; }

	DateTime? SendToServerDate { get; set; }

	decimal Length { get; set; }

	EnProductIndex IndexingType { get; set; }

	EnProductKind Kind { get; set; }

	Guid? TaxGuid { get; set; }

	DateTime? DiscountStartDate { get; set; }

	DateTime? DiscountEndDate { get; set; }

	bool IsDependentOnCurrency { get; set; }

	string MappId { get; set; }

	string NameForDisplay { get; set; }

	decimal DiscountPrice { get; set; }

	Guid? DefaultVariable { get; set; }
}
