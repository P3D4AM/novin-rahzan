using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace SharedAccEntityFramworkPersistence.Entities.Attendance.Biometric;

[Table("Att_BioWriteDevice")]
public class BioWriteDevice : IBioWriteDevice
{
	public Guid BioGuid { get; set; }

	public Guid DeviceGuid { get; set; }

	public int HardwareCode { get; set; }

	[MaxLength(250)]
	public string HarwareName { get; set; }

	public DateTime InsertedDateM { get; set; }

	public short TryCount { get; set; }

	public DateTime NextTry { get; set; }

	public Guid UserGuid { get; set; }

	public string Message { get; set; }

	public EnDeviceMessageType MessageType { get; set; }

	[JsonIgnore]
	public virtual Biometric BioItem { get; set; }
}
