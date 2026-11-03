using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebExecuteQuery : IReceiver, INpDisposable, IDisposable
{
	public string RequestedJsonQuery { get; set; }

	public string JsonResult { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public bool IsDisposed { get; private set; }

	public event Func<object, ConnectionEventArgs<ExecuteQueryRequestEventArgs>, Task> OnExecuteQueryRequest;

	public event Func<object, ConnectionEventArgs<ExecuteQueryResponseEventArgs>, Task> OnExecuteQueryResponse;

	public static List<Packet> GenerateQueryRequest(ExecuteQueryRequestEventArgs queryRequest, Guid packetReferenceId)
	{
		using (new JaegerMonitor("GenerateQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs"))
		{
			try
			{
				return new List<ExecuteQueryRequestEventArgs> { queryRequest }.GeneratePackets(PacketType.ExecuteQuery, 1, null, isSync: false, packetReferenceId, 0, deliveryRequst: true, "GenerateQueryRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateQueryRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs", 37);
				return null;
			}
		}
	}

	public static List<Packet> GenerateQueryResponse(ExecuteQueryResponseEventArgs response)
	{
		using (new JaegerMonitor("GenerateQueryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs"))
		{
			try
			{
				return new List<ExecuteQueryResponseEventArgs> { response }.GeneratePackets(PacketType.ExecuteQuery, 2, null, isSync: false, response.PacketReferenceId, 0, deliveryRequst: true, "GenerateQueryResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateQueryResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs", 52);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			QueryActions queryActions = (QueryActions)action;
			return queryActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs", 66);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((QueryActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs", 79);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs"))
		{
			_ = 1;
			try
			{
				switch ((QueryActions)pack.PacketActions)
				{
				case QueryActions.ExecuteRequest:
				{
					ConnectionEventArgs<ExecuteQueryRequestEventArgs> args2 = new ConnectionEventArgs<ExecuteQueryRequestEventArgs>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnExecuteQueryRequest, this, args2, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs");
					break;
				}
				case QueryActions.ExecuteResponse:
				{
					ConnectionEventArgs<ExecuteQueryResponseEventArgs> args = new ConnectionEventArgs<ExecuteQueryResponseEventArgs>(pack, ConnectionUniqueId, Connection);
					await Utilities.RaiseEvent(this.OnExecuteQueryResponse, this, args, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs");
					break;
				}
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs", 104);
			}
		}
	}

	public void Dispose()
	{
		try
		{
			if (!IsDisposed)
			{
				IsDisposed = true;
				if (this.OnExecuteQueryRequest != null)
				{
					this.OnExecuteQueryRequest = null;
				}
				if (this.OnExecuteQueryResponse != null)
				{
					this.OnExecuteQueryResponse = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebExecuteQuery.cs", 117);
		}
	}
}
