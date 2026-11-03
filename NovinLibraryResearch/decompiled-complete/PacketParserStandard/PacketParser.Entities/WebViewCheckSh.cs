using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.Entities.Interfaces;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebViewCheckSh : Serializable<WebViewCheckSh>, IReceiver, IViewCheckSh, IHasJustGuid, IHasStatus, INpDisposable, IDisposable, ISyncFromClient2Server<WebViewCheckSh>, IDeliverableEntity, IEditableDelivery
{
	public Guid Guid { get; set; }

	public DateTime Modified { get; set; }

	public string Number { get; set; }

	public decimal Price { get; set; }

	public ENCheckSHStatus Status { get; set; }

	public ENCheckSHStatus ChequeStatus
	{
		get
		{
			return Status;
		}
		set
		{
			Status = value;
		}
	}

	public string DateSarresid { get; set; }

	public string BankName { get; set; }

	public string ST { get; set; }

	public string CustomerFullName { get; set; }

	public string PardakhtDate { get; set; }

	[Obsolete("")]
	public int IDSanad { get; set; }

	public Guid GuidSanad { get; set; }

	public bool Aval2re { get; set; }

	public DateTime DateSarresidM { get; set; }

	public string PardakhtDesc { get; set; }

	public DateTime ResetTime { get; set; }

	public Guid BankGuid { get; set; }

	public Guid? A2reMoeinGuid { get; set; }

	public Guid? A2reTafsilGuid { get; set; }

	public Guid CreditMoeinGuid { get; set; }

	public Guid? CreditTafsilGuid { get; set; }

	public bool IsEnteghali { get; set; }

	public bool SayadiRegistered { get; set; }

	public List<WebChequeShOperation> OperationList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public short Version { get; set; }

	public DateTime? DateUtc { get; set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	bool IHasStatus.Status => true;

	public event Func<object, ViewCheckShReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebViewCheckSh>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebViewCheckSh>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebViewCheckSh>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeletedRequest;

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebViewCheckSh> viewCheckSh, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			try
			{
				if (viewCheckSh == null || !viewCheckSh.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (viewCheckSh == null)
				{
					result = null;
				}
				else
				{
					result = viewCheckSh.GeneratePackets(PacketType.ViewCheckSh, 8, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 71);
				throw;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<Guid> viewCheckSh, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			try
			{
				if (viewCheckSh == null || (viewCheckSh != null && viewCheckSh.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (viewCheckSh == null)
				{
					result = null;
				}
				else
				{
					result = viewCheckSh.GeneratePackets(PacketType.ViewCheckSh, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 86);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				ViewCheckShReadEventArgs viewCheckShReadEventArgs = new ViewCheckShReadEventArgs
				{
					Modified = modified
				};
				list.Add(viewCheckShReadEventArgs.ToString());
				return new Packet(PacketType.ViewCheckSh, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 109);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebViewCheckSh> items, short partition, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			try
			{
				if (items == null || (items != null && items.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (items == null)
				{
					result = null;
				}
				else
				{
					result = items.GeneratePackets(PacketType.ViewCheckSh, 6, null, isSync, null, partition, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 125);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebViewCheckSh> ViewCheckShs, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			try
			{
				if (ViewCheckShs == null || (ViewCheckShs != null && ViewCheckShs.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (ViewCheckShs == null)
				{
					result = null;
				}
				else
				{
					result = ViewCheckShs.GeneratePackets(PacketType.ViewCheckSh, 1, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 142);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			try
			{
				if (responses == null || (responses != null && responses.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.ViewCheckSh, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 158);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			ViewCheckShActions viewCheckShActions = (ViewCheckShActions)action;
			return viewCheckShActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 172);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((ViewCheckShActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 185);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs"))
		{
			_ = 5;
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
				switch ((ViewCheckShActions)pack.PacketActions)
				{
				case ViewCheckShActions.SaveRequest:
				{
					ConnectionEventArgs<WebViewCheckSh> args2 = new ConnectionEventArgs<WebViewCheckSh>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs");
					break;
				}
				case ViewCheckShActions.SaveResponse:
				{
					SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs");
					break;
				}
				case ViewCheckShActions.ReadRequest:
				{
					ViewCheckShReadEventArgs viewCheckShReadEventArgs = pack.Args[1].FromJson<ViewCheckShReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 214);
					viewCheckShReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					viewCheckShReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, viewCheckShReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs");
					break;
				}
				case ViewCheckShActions.ReadResponse:
				{
					ConnectionEventArgs<WebViewCheckSh> args3 = new ConnectionEventArgs<WebViewCheckSh>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 228);
						break;
					}
				}
				case ViewCheckShActions.DeleteRequest:
				{
					if (this.OnDeletedRequest == null)
					{
						break;
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeletedRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 238);
						break;
					}
				}
				default:
					throw new Exception($"pack.PacketActions:{pack.PacketActions}//{pack.ToString()}");
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 245);
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
				if (this.OnDeletedRequest != null)
				{
					this.OnDeletedRequest = null;
				}
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckSh.cs", 263);
		}
	}

	public Task<ReturnedSaveFuncInfo> SetDeliveryAsync(WebDelivery del, CancellationToken token, short tryCount = 10, string tranName = "")
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> SetSaveResultAsync(SaveResponse item, CancellationToken token, short tryCount, string tranName)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> ResendNotSents(string contextGuid, CancellationToken token, string tranName, short maxparalelTask)
	{
		throw new NotImplementedException();
	}

	public Task<ReturnedSaveFuncInfo> Send2ServerAsync(CancellationToken token, EnSystem system, Priority priority)
	{
		throw new NotImplementedException();
	}
}
