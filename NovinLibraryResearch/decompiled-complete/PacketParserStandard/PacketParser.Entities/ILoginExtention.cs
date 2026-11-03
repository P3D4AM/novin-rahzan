using System;

namespace PacketParser.Entities;

public interface ILoginExtention
{
	long AndroidIme { get; set; }

	string CpuSerial { get; set; }

	Guid DbGuid { get; set; }

	string HardSerial { get; set; }

	ENSource Source { get; set; }

	long LkSerial { get; set; }

	WebUserInformation UserInfo { get; set; }
}
