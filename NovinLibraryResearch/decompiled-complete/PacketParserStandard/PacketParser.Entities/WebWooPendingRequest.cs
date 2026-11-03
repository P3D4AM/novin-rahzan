using System;
using PacketParser.Entities.Interfaces;

namespace PacketParser.Entities;

public class WebWooPendingRequest : IWooPendingRequest
{
	public short DepId { get; set; }

	public string UserName { get; set; }

	public string Password { get; set; }

	public int Id { get; set; }

	public WooRequestType RequestType { get; set; }

	public Guid EntityGuid { get; set; }

	public object Description { get; set; }

	public string Data { get; set; }
}
