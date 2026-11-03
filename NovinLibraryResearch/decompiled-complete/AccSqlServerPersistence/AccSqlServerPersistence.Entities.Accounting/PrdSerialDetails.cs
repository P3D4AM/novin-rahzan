using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;

namespace AccSqlServerPersistence.Entities.Accounting;

[Table("Acc_PrdSerialDetail")]
public class PrdSerialDetails : IPrdSerialDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid, IHasMetaData
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public Guid DetailGuid { get; set; }

	public Document AsType { get; set; }

	public bool IsVorod { get; set; }

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

	public bool Status => true;

	public short Ordinal { get; set; }

	public DateTime? SanadDate { get; set; }

	public string? MetaData { get; set; }

	public PrdSerial MasterItem { get; set; }
}
