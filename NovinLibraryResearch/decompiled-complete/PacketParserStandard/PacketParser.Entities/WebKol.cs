using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.DefaultsCoding;
using PacketParser.Entities.Interfaces;
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

[Serializable]
public class WebKol : Serializable<WebKol>, IKol, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, IReceiver, INpDisposable, IDisposable, ISyncFromClient2Server<WebKol>, IDeliverableEntity, IEditableDelivery
{
	private WebHesabGroup webHesabGroup;

	private WebHesabType webHesabType;

	private Guid groupGuid;

	private Guid mahiatGuid;

	public Guid Guid { get; set; }

	public string Name { get; set; }

	public Guid GroupGuid
	{
		get
		{
			if (!(groupGuid == Guid.Empty))
			{
				return groupGuid;
			}
			return WebHesabGroup.Guid;
		}
		set
		{
			groupGuid = value;
		}
	}

	public Guid MahiatGuid
	{
		get
		{
			if (!(mahiatGuid == Guid.Empty))
			{
				return GroupGuid;
			}
			return WebHesabType.Guid;
		}
		set
		{
			mahiatGuid = value;
		}
	}

	public ENMahiat Mahiat { get; set; }

	public int Code { get; set; }

	public bool Status { get; set; }

	public string Description { get; set; }

	public bool System { get; set; }

	public decimal Account { get; set; }

	public DateTime DateM { get; set; }

	public DateTime Modified { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public byte[] GardeshHesab { get; set; }

	public WebHesabGroup WebHesabGroup
	{
		get
		{
			if (webHesabGroup == null)
			{
				webHesabGroup = new WebHesabGroup
				{
					Code = 1,
					Guid = ParentDefaults.HesabGroupGuids.DarayiJari,
					Modified = DateTime.Now,
					Name = "گروه حساب پیش فرض",
					Status = true,
					TypeGuid = ParentDefaults.HesabTypeGuids.Darayi
				};
			}
			return webHesabGroup;
		}
		set
		{
			webHesabGroup = value;
		}
	}

	public WebHesabType WebHesabType
	{
		get
		{
			if (webHesabType == null)
			{
				webHesabType = new WebHesabType
				{
					Name = "دارایی",
					Guid = ParentDefaults.HesabTypeGuids.Darayi
				};
			}
			return webHesabType;
		}
		set
		{
			webHesabType = value;
		}
	}

	public List<WebGardeshTafsili> Gardesh { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public event Func<object, HesabKolReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebKol>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebKol>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebKol>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeleteRequest;

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebKol> webKol, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs"))
		{
			try
			{
				if (webKol == null || !webKol.Any())
				{
					throw new NullReferenceException();
				}
				string text = webKol.Select((WebKol p) => $"{p.Guid}/{p.Name}").GenerateDebugDescription(5);
				object result;
				if (webKol == null)
				{
					result = null;
				}
				else
				{
					string extraDataForDebug = text;
					result = webKol.GeneratePackets(PacketType.Kol, 1, null, isSync: false, Guid.NewGuid(), partitionId, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 115);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs"))
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
					result = responses.GeneratePackets(PacketType.Kol, 2, null, isSync: false, null, 0, deliveryRequst: false, extraDataForDebug);
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 131);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				HesabKolReadEventArgs hesabKolReadEventArgs = new HesabKolReadEventArgs
				{
					Modified = modified
				};
				list.Add(hesabKolReadEventArgs.ToString());
				return new Packet(PacketType.Kol, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 157);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebKol> kols, bool isSync, int partition)
	{
		if (kols == null || kols.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs"))
		{
			try
			{
				if (kols == null || kols.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				object result;
				if (kols == null)
				{
					result = null;
				}
				else
				{
					result = kols.GeneratePackets(PacketType.Kol, 6, null, isSync, null, partition, deliveryRequst: false, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 173);
				return null;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> kols)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs"))
		{
			try
			{
				if (kols == null || kols.Count() <= 0)
				{
					throw new NullReferenceException();
				}
				return kols?.GeneratePackets(PacketType.Kol, 8, null, isSync: false, null, 0, deliveryRequst: true, "GenerateDeleteRequest");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 188);
				return null;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			WebKolActions webKolActions = (WebKolActions)action;
			return webKolActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 203);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((WebKolActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 216);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs"))
		{
			_ = 5;
			try
			{
				switch ((WebKolActions)pack.PacketActions)
				{
				case WebKolActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebKol> args4 = new ConnectionEventArgs<WebKol>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs");
					}
					break;
				case WebKolActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs");
					}
					break;
				case WebKolActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						HesabKolReadEventArgs hesabKolReadEventArgs = pack.Args[1].FromJson<HesabKolReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 244);
						hesabKolReadEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						hesabKolReadEventArgs.Connection = connection;
						await Utilities.RaiseEvent(this.OnReadRequest, this, hesabKolReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs");
					}
					break;
				case WebKolActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebKol> args3 = new ConnectionEventArgs<WebKol>(pack, ConnectionUniqueId, connection);
					if (pack.IsSynced)
					{
						if (this.OnSync != null)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs");
						}
						else if (this.OnReadResponse != null)
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs");
						}
					}
					break;
				}
				case WebKolActions.DeleteRequest:
					if (this.OnDeleteRequest != null)
					{
						ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, ConnectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnDeleteRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs");
					}
					break;
				case (WebKolActions)3:
				case (WebKolActions)4:
				case WebKolActions.Reset:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 276);
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebKol.cs", 317);
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
