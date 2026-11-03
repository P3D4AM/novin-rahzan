using PacketParser.Entities;

namespace PacketParser.LkService;

public class AttendancePackageClass
{
	public Level_Hozor LevelHozor { get; set; }

	public string SoftwareSerialHozor { get; set; }

	public long ClientlkSerial { get; set; }

	public long LkSerial { get; set; }
}
