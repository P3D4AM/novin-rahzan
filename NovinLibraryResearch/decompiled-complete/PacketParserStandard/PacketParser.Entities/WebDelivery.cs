using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebDelivery : Serializable<WebDelivery>, IDelivery, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable
{
	private const int ChunckSizeKB = 100;

	private bool _isDisposed;

	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public string ShortMessage { get; set; }

	public string Message { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	[DefaultValue(true)]
	public bool DeliveryStatus { get; set; } = true;


	public EntityPart Part { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public string DateSh => DeliveryDate.MiladiToCalture();

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed
	{
		get
		{
			if (!base.IsDisposed)
			{
				return _isDisposed;
			}
			return true;
		}
	}

	public bool SendToAccounting { get; set; }

	public ServerStatusForClient ServerStatus
	{
		get
		{
			if (!SendToAccounting)
			{
				return ServerStatusForClient.Sent;
			}
			if (DeliveryStatus)
			{
				return ServerStatusForClient.Delivered;
			}
			return ServerStatusForClient.DeliveryError;
		}
	}

	public event Func<object, DeliveryReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebDelivery>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebDelivery>, Task> OnSync;

	public static List<Packet> GenerateReadRequest(IEnumerable<DeliveryReadModel> models)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs"))
		{
			try
			{
				if (models == null || models.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				string text = models.Select((DeliveryReadModel p) => $"{p.Guid}/{p.Entity}").GenerateDebugDescription(5);
				object result;
				if (models == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = models.GeneratePackets(PacketType.Delivery, 1, null, isSync: false, null, 0, deliveryRequst: true, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 63);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebDelivery> items, Guid packetRefrencedId, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs"))
		{
			try
			{
				if (items == null || (items != null && items.Count() == 0))
				{
					throw new NullReferenceException();
				}
				string extraDataForDebug = items.Select((WebDelivery p) => $"Guid:{p.Guid}/DeliveryStatus:{p.DeliveryStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					IEnumerable<WebDelivery> enumerable = items.Where((WebDelivery p) => p == null || p.Part != EntityPart.None);
					if (enumerable == null)
					{
						result = null;
					}
					else
					{
						Guid? packetReferenceId = packetRefrencedId;
						result = enumerable.GeneratePackets(PacketType.Delivery, 2, null, isSync, packetReferenceId, 0, deliveryRequst: false, extraDataForDebug);
					}
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 81);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebDelivery> items, short partition, bool isSync)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs"))
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
					IEnumerable<WebDelivery> enumerable = items.Where((WebDelivery p) => p == null || p.Part != EntityPart.None);
					if (enumerable == null)
					{
						result = null;
					}
					else
					{
						result = enumerable.GeneratePackets(PacketType.Delivery, 2, null, isSync, null, partition, deliveryRequst: false, "GenerateReadResponse");
					}
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 97);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			DeliverytActions deliverytActions = (DeliverytActions)action;
			return deliverytActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 112);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((DeliverytActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 125);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs"))
		{
			_ = 2;
			try
			{
				int num;
				if (connection == null)
				{
					num = 1;
				}
				else
				{
					_ = connection.cancellationToken;
					num = 0;
				}
				if (num != 0)
				{
					return;
				}
				connection?.cancellationToken.ThrowIfCancellationRequested();
				switch ((DeliverytActions)pack.PacketActions)
				{
				case DeliverytActions.ReadResponse:
				{
					ConnectionEventArgs<WebDelivery> args2 = new ConnectionEventArgs<WebDelivery>(pack, connectionUniqueId, connection);
					if (pack.IsSynced)
					{
						await Utilities.RaiseEvent(this.OnSync, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs");
					}
					else
					{
						await Utilities.RaiseEvent(this.OnReadResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs");
					}
					break;
				}
				case DeliverytActions.ReadRequest:
				{
					DeliveryReadEventArgs args = new DeliveryReadEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnReadRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs");
					break;
				}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 165);
			}
		}
	}

	public override void Dispose()
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
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
				base.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDelivery.cs", 185);
		}
	}
}
