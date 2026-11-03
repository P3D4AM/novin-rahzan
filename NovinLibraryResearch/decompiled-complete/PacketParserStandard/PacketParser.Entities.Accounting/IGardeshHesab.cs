using System;
using System.Collections.Generic;

namespace PacketParser.Entities.Accounting;

public interface IGardeshHesab
{
	Guid? AsGuid { get; set; }

	Document AsType { get; set; }

	DocumentDet AsTypeEnum { get; set; }

	string AutoDescriptionShort { get; set; }

	decimal Bazaryab { get; set; }

	decimal Credit { get; set; }

	string CreditForDisplay { get; set; }

	DateTime Date { get; set; }

	string DateSh { get; set; }

	decimal Debit { get; set; }

	string DebitForDisplay { get; set; }

	Guid DecimalGuid { get; set; }

	short depId { get; set; }

	Guid DepotGuid { get; set; }

	string DescForDisplay { get; set; }

	string Description { get; set; }

	decimal Discount { get; set; }

	string DiscountForDisplay { get; }

	decimal DiscountPercent { get; set; }

	Guid Guid { get; set; }

	int Id2 { get; set; }

	bool Ischecked { get; set; }

	bool IsFactor { get; set; }

	Guid KolGuid { get; set; }

	DateTime Modified { get; set; }

	Guid MoeinGuid { get; set; }

	Guid PrdGuid { get; set; }

	IProduct PrdInfo { get; set; }

	decimal Price { get; set; }

	string PriceForDisplay { get; set; }

	decimal QT { get; set; }

	decimal QT2 { get; set; }

	decimal Rem { get; set; }

	string RemForDisplay { get; set; }

	string RowDesc { get; set; }

	string RowSumForDisplay { get; set; }

	string SanadDesc { get; set; }

	int SanadNO { get; set; }

	int SanadSt { get; set; }

	string Serial { get; set; }

	decimal Sood { get; set; }

	Guid? TafsilGuid { get; set; }

	List<GardeshTafsil> LstTafsiliItems { get; set; }

	string TafsiliItems { get; set; }

	string TafsilItemName { get; set; }

	string Tashkhis { get; set; }

	Guid UGuid { get; set; }

	string UserName { get; set; }

	string vahedForDisplay { get; set; }

	Guid VGuid { get; set; }
}
