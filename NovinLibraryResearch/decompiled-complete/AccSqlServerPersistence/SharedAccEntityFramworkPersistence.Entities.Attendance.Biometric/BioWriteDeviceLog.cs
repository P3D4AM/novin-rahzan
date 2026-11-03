using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

[Table("Att_BioWriteDeviceLog")]
public class BioWriteDeviceLog : IBioWriteDeviceLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public DateTime DateM { get; set; }

	public string Description { get; set; }

	public Guid DeviceGuid { get; set; }

	public Guid? BioGuid { get; set; }

	public int HardwareCode { get; set; }

	[MaxLength(255)]
	public string HardwareName { get; set; }

	public ReturnedState Result { get; set; }

	public Guid? UserGuid { get; set; }

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

	[NotMapped]
	public bool Status { get; set; }

	public string BioInfo { get; set; }
}
