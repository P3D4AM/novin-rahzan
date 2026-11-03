using System;
using System.Collections.Generic;

namespace PacketParser.Entities;

public class WebErrorLogStandard : Serializable<WebErrorLogStandard>
{
	public ENSource Source { get; set; }

	public Guid DbGuid { get; set; } = Guid.Empty;


	public string CpuSerial { get; set; } = "";


	public string HardSerial { get; set; } = "";


	public long AndroidIme { get; set; }

	public long LkSerial { get; set; }

	public string Username { get; set; }

	public string Password { get; set; }

	public string Version { get; set; }

	public List<WebErrorLog> ErrorLogs { get; set; }
}
