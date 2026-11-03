using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PacketParser.Entities;

namespace PacketParser.EventArgs;

public class SaveResponseEventArgs : BaseEventArgs
{
	[JsonIgnore]
	public List<SaveResponse> SaveResponses { get; set; }

	public SaveResponseEventArgs()
	{
	}

	public SaveResponseEventArgs(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		base.ConnectionUniqueId = connectionUniqueId;
		base.Connection = connection;
		SaveResponses = Json.FromJson<List<SaveResponse>>(pack.PacketContents, sendErrorIfException: true, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 20);
	}

	public override string ToString()
	{
		try
		{
			return Json.ToStringJson(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 31);
			return "";
		}
	}
}
public class SaveResponseEventArgs<TKey> : BaseEventArgs
{
	[JsonIgnore]
	public List<SaveResponse<TKey>> SaveResponses { get; set; }

	public SaveResponseEventArgs()
	{
	}

	public SaveResponseEventArgs(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		base.ConnectionUniqueId = connectionUniqueId;
		base.Connection = connection;
		SaveResponses = Json.FromJson<List<SaveResponse<TKey>>>(pack.PacketContents, sendErrorIfException: true, ".ctor", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 50);
	}

	public override string ToString()
	{
		try
		{
			return Json.ToStringJson(this);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ToString", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\EventArgs\\SaveResponseEventArgs.cs", 61);
			return "";
		}
	}
}
