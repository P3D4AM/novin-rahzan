using System;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class Send2FriendReadEventArgs : BaseEventArgs
{
	public DateTime Modified { get; set; }

	public Send2FriendReadEventArgs()
	{
	}

	public Send2FriendReadEventArgs(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		base.ConnectionUniqueId = connectionUniqueId;
		Modified = Json.FromJson<DateTime>(pack.PacketContents, sendErrorIfException: true, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Send2Friend\\Send2FriendReadEventArgs.cs", 20);
		connection = base.Connection;
	}

	public override string ToString()
	{
		return Json.ToStringJson(this);
	}
}
