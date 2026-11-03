using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebCusGroup : Serializable<WebCusGroup>, ICusGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, ISyncFromClient2Server<WebCusGroup>, IDeliverableEntity, IEditableDelivery
{
	private const int ChunckSizeKB = 100;

	public short Id { get; set; }

	public string Name { get; set; } = "";


	public string Description { get; set; } = "";


	[DefaultValue(true)]
	public bool Status { get; set; } = true;


	public bool Special { get; set; }

	public Guid Guid { get; set; } = Guid.Empty;


	public DateTime Modified { get; set; }

	public short TafsilLevelId { get; set; }

	public int MoeinDefaultId { get; set; } = 9;


	public DateTime ResetTime { get; set; }

	[JsonIgnore]
	public string SettingData { get; set; }

	public Guid MoeinDefaultGuid { get; set; }

	public Guid TafsilGroupGuid { get; set; }

	[Obsolete]
	public Guid TafsilLevelGuid
	{
		get
		{
			return TafsilGroupGuid;
		}
		set
		{
			TafsilGroupGuid = value;
		}
	}

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public event Func<object, CusGroupReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebCusGroup>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebCusGroup>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebCusGroup>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeleteRequest;

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				CusGroupReadEventArgs cusGroupReadEventArgs = new CusGroupReadEventArgs
				{
					Modified = modified
				};
				list.Add(cusGroupReadEventArgs.ToString());
				return new Packet(PacketType.CustomerGroup, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 84);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebCusGroup> groups, bool isSync = false)
	{
		if (groups == null || groups.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs"))
		{
			try
			{
				if (groups == null || groups.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return groups?.GeneratePackets(PacketType.CustomerGroup, 6, null, isSync, null, 0, deliveryRequst: true, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 101);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> groups)
	{
		if (groups == null || !groups.Any())
		{
			throw new NullReferenceException("CusGroups is null or empty!!");
		}
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs"))
		{
			try
			{
				return groups?.GeneratePackets(PacketType.CustomerGroup, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 117);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebCusGroup> groups, int partition)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs"))
		{
			try
			{
				if (groups == null || !groups.Any())
				{
					throw new NullReferenceException();
				}
				string text = groups.Select((WebCusGroup p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (groups == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = groups.GeneratePackets(PacketType.CustomerGroup, 1, null, isSync: false, null, partition, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 133);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs"))
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
					result = responses.GeneratePackets(PacketType.CustomerGroup, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 150);
				throw;
			}
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((CusGroupActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 189);
			return $"Error({action})";
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			CusGroupActions cusGroupActions = (CusGroupActions)action;
			return cusGroupActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 202);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs"))
		{
			_ = 5;
			try
			{
				switch ((CusGroupActions)pack.PacketActions)
				{
				case CusGroupActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebCusGroup> args4 = new ConnectionEventArgs<WebCusGroup>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs");
					}
					break;
				case CusGroupActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs");
					}
					break;
				case CusGroupActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						CusGroupReadEventArgs cusGroupReadEventArgs = pack.Args[1].FromJson<CusGroupReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 230);
						cusGroupReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						cusGroupReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, cusGroupReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs");
					}
					break;
				case CusGroupActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebCusGroup> args3 = new ConnectionEventArgs<WebCusGroup>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs");
						}
					}
					break;
				}
				case CusGroupActions.DeleteRequest:
					if (this.OnDeleteRequest != null)
					{
						ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs");
					}
					break;
				case (CusGroupActions)3:
				case (CusGroupActions)4:
				case CusGroupActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 265);
			}
		}
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebCusGroup.cs", 288);
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

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxParalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
