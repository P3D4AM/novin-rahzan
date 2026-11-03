using System;
using PacketParser.Entities;

namespace PacketParser.BusinessLayer.Models.Attendance;

public class BioDeviceRelationModel
{
	public Guid DeviceGuid { get; set; }

	public string DeviceName { get; set; }

	public Guid BioGuid { get; set; }

	public string PersonName { get; set; }

	public int HardwareCode { get; set; }

	public EnAttendancePrivilage Privilage { get; set; }

	public string PrivilageForDisplay => Privilage.GetDisplay();

	public DateTime Modified { get; set; }

	public string ModifiedForDisplay => $"{Modified.Hour}:{Modified.Minute}:{Modified.Second} {Modified.MiladiToCalture()}";

	public short FingerPrintCount { get; set; }

	public bool HasPalm { get; set; }

	public bool HasFace { get; set; }

	public bool HasCard { get; set; }

	public bool HasPassword { get; set; }
}
