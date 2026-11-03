using System.Collections.Generic;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class DeliveryReadEventArgs : BaseEventArgs
{
	public List<DeliveryReadModel> Requests { get; set; }

	public DeliveryReadEventArgs()
	{
	}

	public DeliveryReadEventArgs(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		base.ConnectionUniqueId = connectionUniqueId;
		base.Connection = connection;
		Requests = Json.FromJson<List<DeliveryReadModel>>(pack.PacketContents, sendErrorIfException: true, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\Delivery\\DeliveryReadEventArgs.cs", 30);
	}
}
