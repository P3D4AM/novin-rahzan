using System;
using PacketParser.Entities;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Attendance.Biometric;

namespace PacketParser.BusinessLayer.Models.Attendance;

public class BioWriteDeviceLogModel : IBioWriteDeviceLog, IHasGuid, IHasModified, IHasStatus, IHasJustGuid
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; } = DateTime.Now;


	[Obsolete("", true)]
	public bool Status { get; set; }

	public DateTime DateM { get; set; }

	public string DateSh => DateM.MiladiToCalture();

	public string Time => $"{DateM.Hour}:{DateM.Minute}:{DateM.Second}";

	public string Description { get; set; }

	public Guid DeviceGuid { get; set; }

	public string DeviceName { get; set; }

	public Guid? BioGuid { get; set; }

	public string PersonName { get; set; }

	public int HardwareCode { get; set; }

	public string HardwareName { get; set; }

	public ReturnedState Result { get; set; }

	public string ResultForDisplay
	{
		get
		{
			if (Result == ReturnedState.Success)
			{
				return "موفق";
			}
			if (Result == ReturnedState.None)
			{
				return "ناموفق";
			}
			return "";
		}
	}

	public Guid? UserGuid { get; set; }
}
