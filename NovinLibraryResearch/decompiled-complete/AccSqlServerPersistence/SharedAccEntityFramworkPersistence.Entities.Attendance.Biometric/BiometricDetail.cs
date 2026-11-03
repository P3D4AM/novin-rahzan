using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

[Table("Att_BiometricDetail")]
public class BiometricDetail : IBiometricDetail, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IHasMasterGuid
{
	[NotMapped]
	private DateTime _modified;

	[Key]
	public Guid Guid { get; set; }

	public Guid MasterGuid { get; set; }

	public EnrolType EnrolType { get; set; }

	public int Ordinal { get; set; }

	[Required]
	public string? Data { get; set; }

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

	[JsonIgnore]
	public virtual Biometric BioItem { get; set; }
}
