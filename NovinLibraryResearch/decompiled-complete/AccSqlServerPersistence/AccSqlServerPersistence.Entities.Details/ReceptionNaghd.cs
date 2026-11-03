using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using AccSqlServerPersistence.Entities.Accounting;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;

namespace AccSqlServerPersistence.Entities.Details;

[Table("Acc_ReceptionNaghd")]
public class ReceptionNaghd : IReceptionNaghd, IParDarDet, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	private DateTime _modified;

	private DateTime _dateM;

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

	public DateTime DateM
	{
		get
		{
			return _dateM;
		}
		set
		{
			DateTime dateM = ((value < DateTime.Now.AddYears(-20)) ? DateTime.Now : value);
			_dateM = dateM;
		}
	}

	[Required]
	public string Description { get; set; }

	public decimal Price { get; set; }

	public Guid TafsilGuid { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid? DecimalGuid { get; set; }

	public Guid MasterGuid { get; set; }

	[NotMapped]
	public string ShortDescription { get; set; }

	public int Ordinal { get; set; }

	[Required]
	public DateTime? DateUtc { get; set; }

	public Tafsil TafsilItem { get; set; }

	public Moein MoeinItem { get; set; }

	public Reception ReceptionItem { get; set; }

	public DecimalEntity DeciamlItem { get; set; }
}
