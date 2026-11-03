using System;

namespace PacketParser.Entities.MetaData;

public class TicketMetaData
{
	public Guid PartGuid { get; set; }

	public ENSource Source { get; set; }

	public long LkSerial { get; set; }

	public int DepId { get; set; }

	public int UserId { get; set; }

	public string UserName { get; set; }

	public string AndroidIme { get; set; }

	public long Number { get; set; }
}
