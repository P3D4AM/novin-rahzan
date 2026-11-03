using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebTicketPart : Serializable<WebTicketPart>, ITicketPart, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private bool _isDisposed;

	public Guid Guid { get; set; }

	public bool Status { get; set; } = true;


	public DateTime Modified { get; set; } = DateTime.Now;


	public string Name { get; set; }

	public Guid? ParentGuid { get; set; }

	public string Description { get; set; }

	public short Ordinal { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!_isDisposed)
			{
				return base.IsDisposed;
			}
			return true;
		}
	}

	public event Func<object, TicketPartEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebTicketPart>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebTicketPart>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebTicketPart>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)11).ToString() };
				TicketPartEventArgs ticketPartEventArgs = new TicketPartEventArgs
				{
					Modified = modified
				};
				list.Add(ticketPartEventArgs.ToString());
				return new Packet(PacketType.TicketPart, 11, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 44);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebTicketPart> items, int partitionId, bool isSync)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs"))
		{
			try
			{
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.TicketPart, 12, null, isSync, null, partitionId, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 58);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebTicketPart> items, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs"))
		{
			try
			{
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.TicketPart, 9, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 71);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.TicketPart, 10, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 84);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebTicketPart> items)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.TicketPart, 13, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 97);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		TicketPartActions ticketPartActions = (TicketPartActions)action;
		return ticketPartActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((TicketPartActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs"))
		{
			_ = 4;
			try
			{
				switch ((TicketPartActions)pack.PacketActions)
				{
				case TicketPartActions.SaveRequest:
				{
					ConnectionEventArgs<WebTicketPart> args3 = new ConnectionEventArgs<WebTicketPart>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs");
					break;
				}
				case TicketPartActions.SaveResponse:
				{
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs");
					break;
				}
				case TicketPartActions.ReadRequest:
				{
					TicketPartEventArgs ticketPartEventArgs = pack.Args[1].FromJson<TicketPartEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 120);
					ticketPartEventArgs.ConnectionUniqueId = connectionUniqueId;
					ticketPartEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, ticketPartEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs");
					break;
				}
				case TicketPartActions.ReadResponse:
				{
					ConnectionEventArgs<WebTicketPart> args = new ConnectionEventArgs<WebTicketPart>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs");
					}
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
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 133);
			}
		}
	}

	public override void Dispose()
	{
		Dispose(disposing: true);
	}

	public override void Dispose(bool disposing)
	{
		try
		{
			if (!_isDisposed)
			{
				_isDisposed = true;
				if (this.OnReadRequest != null)
				{
					this.OnReadRequest = null;
				}
				if (this.OnReadResponse != null)
				{
					this.OnReadResponse = null;
				}
				if (this.OnSaveRequest != null)
				{
					this.OnSaveRequest = null;
				}
				if (this.OnSaveResponse != null)
				{
					this.OnSaveResponse = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTicketPart.cs", 149);
		}
	}
}
