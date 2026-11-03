using System;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

[Table("Att_BioDeviceRelation")]
public class BioDeviceRelation : IBioDeviceRelation
{
	[NotMapped]
	private DateTime _modified;

	public Guid BioGuid { get; set; }

	public Guid DeviceGuid { get; set; }

	public int HardwareCode { get; set; }

	public EnAttendancePrivilage Privilage { get; set; }

	public DateTime? FromDateM { get; set; }

	public DateTime? ToDateM { get; set; }

	public bool Status { get; set; }

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

	[JsonIgnore]
	public virtual Biometric BioItem { get; set; }
}
