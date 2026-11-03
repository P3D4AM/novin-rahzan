using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace PacketParser.BusinessLayer.Models.Attendance;

public class BioWriteDeviceModel : IBioWriteDevice
{
	public Guid BioGuid { get; set; }

	public string PersonName { get; set; }

	public Guid DeviceGuid { get; set; }

	public string DeviceName { get; set; }

	public EnDeviceGroup DeviceGroup { get; set; }

	public string DeviceGroupForDisplay => DeviceGroup.GetDisplay();

	public int HardwareCode { get; set; }

	public string HarwareName { get; set; }

	public DateTime InsertedDateM { get; set; }

	public string InsertedDateSh => $"{InsertedDateM.Hour}:{InsertedDateM.Minute}:{InsertedDateM.Second} {InsertedDateM.MiladiToCalture()}";

	public short TryCount { get; set; }

	public DateTime NextTry { get; set; }

	public string NextTrySh => $"{NextTry.Hour}:{NextTry.Minute}:{NextTry.Second} {NextTry.MiladiToCalture()}";

	public Guid UserGuid { get; set; }

	public string Message { get; set; }

	public short FingerPrintCount { get; set; }

	public bool HasPalm { get; set; }

	public bool HasFace { get; set; }

	public bool HasCard { get; set; }

	public bool HasPassword { get; set; }

	public EnDeviceMessageType MessageType { get; set; }
}
