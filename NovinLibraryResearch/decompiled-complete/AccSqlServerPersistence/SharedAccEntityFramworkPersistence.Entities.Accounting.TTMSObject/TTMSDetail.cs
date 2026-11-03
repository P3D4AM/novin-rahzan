using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting.TTMS;

namespace SharedAccEntityFramworkPersistence.Entities.Accounting.TTMSObject;

[Table("Acc_TTMSDetail")]
public class TTMSDetail : ITTMSDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

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

	public Guid AsGuid { get; set; }

	public Document AsType { get; set; }

	public string Description { get; set; }

	[NotMapped]
	public bool Status { get; set; }

	public TTMS MasterItem { get; set; }
}
