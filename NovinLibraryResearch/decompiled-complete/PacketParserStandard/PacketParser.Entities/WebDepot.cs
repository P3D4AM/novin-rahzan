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
public class WebDepot : Serializable<WebDepot>, IDepot, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, IDeliverableEntity
{
	public short Id { get; set; }

	public string Name { get; set; } = "";


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	[DefaultValue(true)]
	[Obsolete("Use NegativeAmount")]
	public bool Manfi { get; set; } = true;


	public ReturnedState NegativeAmount { get; set; }

	public string Description { get; set; } = "";


	public Guid Guid { get; set; } = Guid.NewGuid();


	public DateTime Modified { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public event Func<object, DepotReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebDepot>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebDepot>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebDepot>, Task> OnSync;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				DepotReadEventArgs depotReadEventArgs = new DepotReadEventArgs
				{
					Modified = modified
				};
				list.Add(depotReadEventArgs.ToString());
				return new Packet(PacketType.Depot, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 51);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebDepot> items, bool isSync = false)
	{
		if (items == null || !items.Any())
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.Depot, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 65);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebDepot> depots, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs"))
		{
			if (depots == null || !depots.Any())
			{
				throw new NullReferenceException();
			}
			try
			{
				object result;
				if (depots == null)
				{
					result = null;
				}
				else
				{
					result = depots.GeneratePackets(PacketType.Depot, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 79);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs"))
		{
			if (responses == null || !responses.Any())
			{
				throw new NullReferenceException();
			}
			try
			{
				return responses.GeneratePackets(PacketType.Depot, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 93);
				return null;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebDepot> depots)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs"))
		{
			if (depots == null || !depots.Any())
			{
				throw new NullReferenceException();
			}
			try
			{
				return depots.GeneratePackets(PacketType.Depot, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 107);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		DepotActions depotActions = (DepotActions)action;
		return depotActions.ToString();
	}

	public string GetPacketActionName(short action)
	{
		return ((DepotActions)action).GetDisplay();
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs"))
		{
			int line = 1;
			try
			{
				line = 2;
				if (pack == null)
				{
					return;
				}
				line = 3;
				switch ((DepotActions)pack.PacketActions)
				{
				case DepotActions.SaveRequest:
				{
					line = 50;
					ConnectionEventArgs<WebDepot> args3 = new ConnectionEventArgs<WebDepot>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs");
					break;
				}
				case DepotActions.SaveResponse:
				{
					line = 70;
					SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs");
					break;
				}
				case DepotActions.ReadRequest:
				{
					line = 90;
					DepotReadEventArgs depotReadEventArgs = pack.Args[1].FromJson<DepotReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 137);
					depotReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					depotReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, depotReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs");
					break;
				}
				case DepotActions.ReadResponse:
				{
					line = 110;
					ConnectionEventArgs<WebDepot> args = new ConnectionEventArgs<WebDepot>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs");
						}
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 152);
						break;
					}
				}
				case (DepotActions)3:
				case (DepotActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, $"Line{line}", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 157);
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
			if (!IsDisposed)
			{
				IsDisposed = true;
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebDepot.cs", 173);
		}
	}
}
