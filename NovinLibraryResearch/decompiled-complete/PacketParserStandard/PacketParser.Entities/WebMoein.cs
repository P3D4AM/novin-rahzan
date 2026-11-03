using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebMoein : Serializable<WebMoein>, IMoein, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, IDeliverableEntity
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public Guid KolGuid { get; set; }

	public long Code { get; set; }

	public bool Status { get; set; }

	public string Description { get; set; }

	public ENMahiat Mahiat { get; set; }

	public bool System { get; set; }

	public bool Peygiri { get; set; }

	public bool Tedadi { get; set; }

	public decimal Account { get; set; }

	[Obsolete("removed!", true)]
	public int CodeKol { get; set; }

	public DateTime DateM { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public byte[] GardeshHesab { get; set; }

	public List<WebGardeshTafsili> Gardesh { get; set; }

	public List<WebMoeinTafsilGroupRelation> TafsilGroupRelationList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, HesabMoeinReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebMoein>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebMoein>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebMoein>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeleteRequest;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebMoein> webMoein, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs"))
		{
			try
			{
				if (webMoein == null || !webMoein.Any())
				{
					throw new NullReferenceException();
				}
				string text = webMoein.Select((WebMoein p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (webMoein == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = webMoein.GeneratePackets(PacketType.Moein, 1, null, isSync: false, null, partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 69);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs"))
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
					result = responses.GeneratePackets(PacketType.Moein, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 85);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				HesabMoeinReadEventArgs hesabMoeinReadEventArgs = new HesabMoeinReadEventArgs
				{
					Modified = modified
				};
				list.Add(hesabMoeinReadEventArgs.ToString());
				return new Packet(PacketType.Moein, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 108);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebMoein> moeins, bool isSync, int partitionId)
	{
		if (moeins == null || moeins.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs"))
		{
			try
			{
				if (moeins == null || moeins.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (moeins == null)
				{
					result = null;
				}
				else
				{
					result = moeins.GeneratePackets(PacketType.Moein, 6, null, isSync, null, partitionId, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 124);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> moeins)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs"))
		{
			try
			{
				if (moeins == null || moeins.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return moeins?.GeneratePackets(PacketType.Moein, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 139);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			HesabMoeinActions hesabMoeinActions = (HesabMoeinActions)action;
			return hesabMoeinActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 153);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((HesabMoeinActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 166);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs"))
		{
			_ = 5;
			try
			{
				switch ((HesabMoeinActions)pack.PacketActions)
				{
				case HesabMoeinActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebMoein> args4 = new ConnectionEventArgs<WebMoein>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs");
					}
					break;
				case HesabMoeinActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs");
					}
					break;
				case HesabMoeinActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						HesabMoeinReadEventArgs hesabMoeinReadEventArgs = pack.Args[1].FromJson<HesabMoeinReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 194);
						hesabMoeinReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						hesabMoeinReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, hesabMoeinReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs");
					}
					break;
				case HesabMoeinActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebMoein> args3 = new ConnectionEventArgs<WebMoein>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs");
						}
					}
					break;
				}
				case HesabMoeinActions.DeleteRequest:
					if (this.OnDeleteRequest != null)
					{
						ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs");
					}
					break;
				case (HesabMoeinActions)3:
				case (HesabMoeinActions)4:
				case HesabMoeinActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 223);
			}
		}
	}

	public override void Dispose()
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebMoein.cs", 241);
		}
	}
}
