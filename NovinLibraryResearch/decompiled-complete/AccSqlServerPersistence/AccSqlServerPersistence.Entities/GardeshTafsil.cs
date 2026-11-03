using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_GardeshTafsil")]
public class GardeshTafsil : Serializable<IGardeshTafsil>, IGardeshTafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
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
			DateTime modified = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now.AddYears(-20) : value);
			_modified = modified;
		}
	}

	public bool Status { get; set; } = true;


	public DateTime Date { get; set; }

	public Guid? DepotGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public Guid? AsGuid { get; set; }

	public short AsType { get; set; }

	public Document EnAsType { get; set; }

	public DocumentDet AsTypeEnum { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public int SanadNO { get; set; }

	public int SanadSt { get; set; }

	public string? Serial { get; set; }

	public decimal Price { get; set; }

	public decimal Discount { get; set; }

	public decimal DiscountPercent { get; set; }

	public int Id2 { get; set; }

	public decimal QT { get; set; }

	public decimal QT2 { get; set; }

	public decimal Bazaryab { get; set; }

	public Guid? VGuid { get; set; }

	public Guid? PrdGuid { get; set; }

	[Required]
	public string SanadDesc { get; set; }

	public DateTime DateMoaser { get; set; }

	public decimal Rem { get; set; }

	public Guid? DecimalGuid { get; set; }

	[Required]
	public string RowDesc { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid KolGuid { get; set; }

	public Guid UGuid { get; set; }

	public decimal DecimalPrice { get; set; }

	public decimal DecimalQt { get; set; }

	public Guid DetailGuid { get; set; }

	public Depot Depot { get; set; }

	public ProductVahed ProductVahed { get; set; }

	public Product Product { get; set; }

	public DecimalEntity DecimalEntity { get; set; }

	public Tafsil Tafsil { get; set; }

	public Moein Moein { get; set; }

	public Kol Kol { get; set; }

	public Cashier Cashier { get; set; }

	[NotMapped]
	public int RowIndex { get; set; }
}
