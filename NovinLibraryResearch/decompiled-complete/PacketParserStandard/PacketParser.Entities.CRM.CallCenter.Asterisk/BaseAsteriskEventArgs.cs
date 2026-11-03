using System;
using System.Collections.Generic;

namespace PacketParser.Entities.CRM.CallCenter.Asterisk;

public abstract class BaseAsteriskEventArgs
{
	public string FullAsterNetEventJsonData { get; set; }

	public string Uuid { get; set; }

	public string LinkedUuid { get; set; }

	public DateTime DateReceived { get; set; }

	public Dictionary<string, string> Attributes { get; set; }

	public string Context { get; set; }

	public string CallerIdNum { get; set; }

	public string Connectedlinenum { get; set; }
}
