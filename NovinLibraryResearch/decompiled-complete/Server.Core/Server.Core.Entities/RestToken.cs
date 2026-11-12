using System;
using PacketParser.Entities;

namespace Server.Core.Entities;

public class RestToken
{
	public string Token { get; set; }

	public short DepId { get; set; }

	public Guid UserAccGuid { get; set; }

	public int DepUserId { get; set; }

	public Guid TafsilGuid { get; set; }

	public DateTime ExpirationDate { get; set; }

	public DateTime CreationDate { get; set; }

	public ENSource Source { get; set; }

	public EnLoginResult Result { get; set; }

	public int HashID { get; set; }
}
