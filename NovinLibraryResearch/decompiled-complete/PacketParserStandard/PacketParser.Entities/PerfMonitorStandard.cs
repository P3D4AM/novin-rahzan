using System;
using System.Collections.Generic;

namespace PacketParser.Entities;

public class PerfMonitorStandard : IDisposable
{
	public ENSource Source { get; set; }

	public Guid DbGuid { get; set; } = Guid.Empty;


	public string CpuSerial { get; set; } = "";


	public string HardSerial { get; set; } = "";


	public long AndroidIme { get; set; }

	public long LkSerial { get; set; }

	public string Username { get; set; } = "";


	public string Password { get; set; } = "";


	public int Version { get; set; }

	public int MonitorVersion { get; set; }

	public string VersionName { get; set; }

	public List<WebPerfMonitor> Monitors { get; set; }

	public void Dispose()
	{
		Monitors = null;
	}
}
