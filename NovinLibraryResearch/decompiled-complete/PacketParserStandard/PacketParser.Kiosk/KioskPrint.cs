using System;
using Newtonsoft.Json;

namespace PacketParser.Kiosk;

public class KioskPrint
{
	[JsonIgnore]
	public const string Key = "KioskPrint";

	public Guid OrderGuid { get; set; }
}
