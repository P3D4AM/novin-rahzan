using System;

namespace PacketParser.Entities.Department;

public class OnlineUsersDep
{
	public ENSource Source { get; set; }

	public DateTime StartConnectoin { get; set; }

	public string UserName { get; set; }

	public string Version { get; set; }

	public string Name { get; set; }
}
