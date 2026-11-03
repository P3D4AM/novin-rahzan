using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebTafsil : Serializable<WebTafsil>, ITafsil, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, ISyncFromClient2Server<WebTafsil>, IDeliverableEntity, IEditableDelivery
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public long Code { get; set; }

	public bool Status { get; set; }

	public string Description { get; set; }

	public bool System { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	public HesabType HType { get; set; }

	[Obsolete]
	public decimal Account { get; set; }

	public DateTime DateM { get; set; }

	public int HID { get; set; }

	public int HTreeID { get; set; }

	public string SerializedData { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public decimal SumAghsat { get; set; }

	public List<byte> GardeshHesab { get; set; }

	public Guid TafsilLevelGuid
	{
		get
		{
			return TafsilGroupGuid;
		}
		set
		{
			if (value != Guid.Empty && TafsilGroupGuid == Guid.Empty)
			{
				TafsilGroupGuid = value;
			}
		}
	}

	[Obsolete("", true)]
	public List<WebGardeshTafsili> Gardesh { get; set; }

	public List<Guid> RelatedMoeinGuidList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public int Version { get; set; }

	public int ClientVerion { get; set; }

	public bool IsSpecial { get; set; }

	public short DepId { get; set; }

	public event Func<object, HesabTafsilReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebTafsil>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebTafsil>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebTafsil>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeleteRequest;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebTafsil> tafsils, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs"))
		{
			try
			{
				if (tafsils == null || !tafsils.Any())
				{
					throw new NullReferenceException();
				}
				string text = tafsils.Select((WebTafsil p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (tafsils == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = tafsils.GeneratePackets(PacketType.Tafsil, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 84);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs"))
		{
			try
			{
				if (responses == null || responses.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				string text = responses.Select((SaveResponse p) => $"Guid:{p.Guid}/Success:{p.SaveResultStatus}/{p.Message}").GenerateDebugDescription(5);
				object result;
				if (responses == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = responses.GeneratePackets(PacketType.Tafsil, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 100);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				HesabMoeinReadEventArgs hesabMoeinReadEventArgs = new HesabMoeinReadEventArgs
				{
					Modified = modified
				};
				list.Add(hesabMoeinReadEventArgs.ToString());
				return new Packet(PacketType.Tafsil, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 123);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebTafsil> tafsils, bool isSync, int partitionId)
	{
		if (tafsils == null || tafsils.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs"))
		{
			try
			{
				if (tafsils == null || tafsils.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (tafsils == null)
				{
					result = null;
				}
				else
				{
					result = tafsils.GeneratePackets(PacketType.Tafsil, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 139);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> tafsils)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs"))
		{
			try
			{
				if (tafsils == null || tafsils.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return tafsils?.GeneratePackets(PacketType.Tafsil, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 153);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			HesabTafsilActions hesabTafsilActions = (HesabTafsilActions)action;
			return hesabTafsilActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 168);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((HesabTafsilActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 181);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs"))
		{
			_ = 5;
			try
			{
				switch ((HesabTafsilActions)pack.PacketActions)
				{
				case HesabTafsilActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebTafsil> args4 = new ConnectionEventArgs<WebTafsil>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs");
					}
					break;
				case HesabTafsilActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs");
					}
					break;
				case HesabTafsilActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						HesabTafsilReadEventArgs hesabTafsilReadEventArgs = pack.Args[1].FromJson<HesabTafsilReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 209);
						hesabTafsilReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						hesabTafsilReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, hesabTafsilReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs");
					}
					break;
				case HesabTafsilActions.ReadResponse:
					if (this.OnReadResponse != null || this.OnSync != null)
					{
						ConnectionEventArgs<WebTafsil> args3 = new ConnectionEventArgs<WebTafsil>(pack, ConnectionUniqueId, connection);
						if (pack.IsSynced && this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs");
						}
						else if (!pack.IsSynced && this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs");
						}
					}
					break;
				case HesabTafsilActions.DeleteRequest:
					if (this.OnDeleteRequest != null)
					{
						ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs");
					}
					break;
				case (HesabTafsilActions)3:
				case (HesabTafsilActions)4:
				case HesabTafsilActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 237);
			}
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public new void Dispose()
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
				if (this.OnDeleteRequest != null)
				{
					this.OnDeleteRequest = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
				if (this.OnDeleteRequest != null)
				{
					this.OnDeleteRequest = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsil.cs", 274);
		}
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
