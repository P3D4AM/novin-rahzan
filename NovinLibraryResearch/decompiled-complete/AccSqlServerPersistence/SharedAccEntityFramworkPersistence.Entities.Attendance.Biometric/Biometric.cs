using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

[Table("Att_Biometric")]
public class Biometric : IBiometric, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IDeliverableEntity
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

	[NotMapped]
	public bool Status { get; set; }

	public EnDeviceGroup DeviceGroup { get; set; }

	public Guid? PersonGuid { get; set; }

	public short FingerPrintCount { get; set; }

	public bool HasPalm { get; set; }

	public bool HasFace { get; set; }

	public bool HasCard { get; set; }

	public bool HasPassword { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public ICollection<BiometricDetail> BiometricDetails { get; set; }

	public ICollection<BioDeviceRelation> BioDeviceRelations { get; set; }

	public ICollection<BioWriteDevice> BioWriteDevices { get; set; }
}
