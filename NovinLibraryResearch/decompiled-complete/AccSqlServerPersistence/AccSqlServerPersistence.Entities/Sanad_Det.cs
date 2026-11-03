using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities;

[Table("Acc_SanadDet")]
public class Sanad_Det : ISanad_Det, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMetaData
{
	[Key]
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	[NotMapped]
	public bool Status { get; }

	public Guid MoeinGuid { get; set; }

	public Guid? TafsilGuid { get; set; }

	public Guid? TafsilGuid2 { get; set; }

	public Guid? TafsilGuid3 { get; set; }

	public Guid? TafsilGuid4 { get; set; }

	public decimal Debit { get; set; }

	public decimal Credit { get; set; }

	public Guid? DecimalGuid { get; set; }

	[Required]
	public string Description { get; set; }

	public decimal Qt { get; set; }

	public string? Serial { get; set; }

	public Document AsType { get; set; }

	public Guid AsGuid { get; set; }

	public DateTime? DateSarresid { get; set; }

	public int Ordinal { get; set; }

	public string? MetaData { get; set; }

	[NotMapped]
	public string DescriptionForUi => "";

	[NotMapped]
	public string RowDesc
	{
		get
		{
			return Description;
		}
		set
		{
			Description = value;
		}
	}

	public Moein Moein { get; set; }

	public Tafsil Tafsil { get; set; }

	public Tafsil Tafsil2 { get; set; }

	public Tafsil Tafsil3 { get; set; }

	public Tafsil Tafsil4 { get; set; }

	public DecimalEntity Decimal { get; set; }
}
