using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Details;

[Table("Acc_PardakhtCheckM")]
public class PardakhtCheckM : IPardakhtCheckM, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
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

	[Required]
	public string Description { get; set; }

	public Guid GuidCheck { get; set; }

	public decimal Price { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	[MaxLength(100)]
	public string? SayadiNationalCode { get; set; }

	[Required]
	[MaxLength(100)]
	public string SayadiName { get; set; }

	public bool SayadiRegistered { get; set; }

	public int Ordinal { get; set; }

	public ReceptionCheck CheckItem { get; set; }

	public Pardakht PardakhtItem { get; set; }

	public Tafsil CreditTafsil { get; set; }

	public Moein CreditMoein { get; set; }
}
