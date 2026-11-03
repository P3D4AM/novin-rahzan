using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class ConnectionEventArgs<T> : BaseEventArgs
{
	[JsonIgnore]
	public List<T> ConnectionData { get; set; }

	public int PartitionId { get; set; }

	public bool IsTcpEvent { get; set; }

	public ConnectionEventArgs()
	{
	}

	public ConnectionEventArgs(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		base.ConnectionUniqueId = ConnectionUniqueId;
		base.Connection = connection;
		base.Part = pack.Part;
		base.TotalParts = pack.TotalParts;
		base.PacketReferenceId = pack.PacketReferenceId;
		ConnectionData = Json.FromJson<List<T>>(pack.PacketContents, sendErrorIfException: true, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ConnectionEventArgs.cs", 27);
		PartitionId = pack.PartitionId;
	}

	public override string ToString()
	{
		using (new JaegerMonitor("ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ConnectionEventArgs.cs"))
		{
			try
			{
				return Json.ToStringJson(this);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\ConnectionEventArgs.cs", 40);
				return "";
			}
		}
	}
}
