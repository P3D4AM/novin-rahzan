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

public class WebViewCheckM : Serializable<WebViewCheckM>, IReceiver, IViewCheckM, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable, ISyncFromClient2Server<WebViewCheckM>, IDeliverableEntity, IEditableDelivery
{
	public Guid Guid { get; set; }

	public DateTime ResetTime { get; set; }

	public DateTime Modified { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	bool IHasStatus.Status => Status == ENUMCheck.BatelShode;

	public string BankName { get; set; }

	public decimal Price { get; set; }

	public ENUMCheck Status { get; set; }

	public string STName { get; set; }

	public string DateSanad { get; set; }

	public DateTime DateSanadM { get; set; }

	public string DateCheck { get; set; }

	public DateTime DateCheckM { get; set; }

	public string DateBargashtORVosol { get; set; }

	public DateTime DateBargashtORVosolM { get; set; }

	public string DateToPlace { get; set; }

	public string Place { get; set; }

	public string Description { get; set; }

	public string Number { get; set; }

	[Obsolete("")]
	public int IDSanad { get; set; }

	public Guid GuidSanad { get; set; }

	public Guid PardakhtGuid { get; set; }

	public int PardakhtId { get; set; }

	public bool Personal { get; set; }

	public string HesabNum { get; set; }

	public string CustomerFullName { get; set; }

	public bool Aval2re { get; set; }

	public long PoshtNomreh { get; set; }

	public Guid VisitorGuid { get; set; }

	public Guid PardazandeTafsilGuid { get; set; }

	public bool IsEnteghali { get; set; }

	public short Version { get; set; }

	public string Sayadi_Name { get; set; }

	public string Sayadi_Identity { get; set; }

	public string Sayadi_NationalCode { get; set; }

	public bool SayadiRegistered { get; set; }

	public DateTime? DateUtc { get; set; }

	public List<WebChequeMOperation> OperationList { get; set; }

	[XmlIgnore]
	[JsonIgnore]
	public new bool IsDisposed { get; private set; }

	public ServerStatusForClient ServerStatus { get; set; }

	public DateTime? DeliveryDate { get; set; }

	public int DeliveryId { get; set; }

	public string DeliveryMessage { get; set; }

	public string DeliveryShortMessage { get; set; }

	public string HostRefrenced { get; set; }

	public event Func<object, ViewCheckMReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebViewCheckM>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebViewCheckM>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebViewCheckM>, Task> OnSync;

	public event Func<object, ConnectionEventArgs<Guid>, Task> OnDeletedRequest;

	public static List<Packet> GenerateDeleteRequest(List<Guid> viewCheckM, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
		{
			try
			{
				if (viewCheckM == null || (viewCheckM != null && viewCheckM.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (viewCheckM == null)
				{
					result = null;
				}
				else
				{
					result = viewCheckM.GeneratePackets(PacketType.ViewCheckM, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 79);
				throw;
			}
		}
	}

	public static List<Packet> GenerateDeleteRequest(IEnumerable<WebViewCheckM> viewCheckM, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
		{
			try
			{
				if (viewCheckM == null || (viewCheckM != null && viewCheckM.Count() == 0))
				{
					throw new NullReferenceException();
				}
				object result;
				if (viewCheckM == null)
				{
					result = null;
				}
				else
				{
					result = viewCheckM.GeneratePackets(PacketType.ViewCheckM, 8, null, isSync: false, null, partitionId, deliveryRequst: true, "GenerateDeleteRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 94);
				throw;
			}
		}
	}

	public static Packet GenerateReadRequest(DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				ViewCheckMReadEventArgs viewCheckMReadEventArgs = new ViewCheckMReadEventArgs
				{
					Modified = modified
				};
				list.Add(viewCheckMReadEventArgs.ToString());
				return new Packet(PacketType.ViewCheckM, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 116);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebViewCheckM> items, short partition, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
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
					result = items.GeneratePackets(PacketType.ViewCheckM, 6, null, isSync, null, partition, deliveryRequst: true, "GenerateReadResponse");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 132);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebViewCheckM> viewCheckMs, int partitionId)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
		{
			try
			{
				if (viewCheckMs == null || !viewCheckMs.Any())
				{
					throw new NullReferenceException();
				}
				object result;
				if (viewCheckMs == null)
				{
					result = null;
				}
				else
				{
					result = viewCheckMs.GeneratePackets(PacketType.ViewCheckM, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 147);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
		{
			try
			{
				if (responses == null || (responses != null && responses.Count() == 0))
				{
					throw new NullReferenceException();
				}
				return responses?.GeneratePackets(PacketType.ViewCheckM, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 165);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			ViewCheckMActions viewCheckMActions = (ViewCheckMActions)action;
			return viewCheckMActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 183);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((ViewCheckMActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 196);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs"))
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
				switch ((ViewCheckMActions)pack.PacketActions)
				{
				case ViewCheckMActions.SaveRequest:
				{
					ConnectionEventArgs<WebViewCheckM> args2 = new ConnectionEventArgs<WebViewCheckM>(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveRequest, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs");
					break;
				}
				case ViewCheckMActions.SaveResponse:
				{
					SaveResponseEventArgs args4 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
					await Utilities.RaiseEvent(this.OnSaveResponse, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs");
					break;
				}
				case ViewCheckMActions.ReadRequest:
				{
					ViewCheckMReadEventArgs viewCheckMReadEventArgs = pack.Args[1].FromJson<ViewCheckMReadEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 224);
					viewCheckMReadEventArgs.ConnectionUniqueId = connectionUniqueId;
					viewCheckMReadEventArgs.Connection = connection;
					await Utilities.RaiseEvent(this.OnReadRequest, this, viewCheckMReadEventArgs, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs");
					break;
				}
				case ViewCheckMActions.ReadResponse:
				{
					ConnectionEventArgs<WebViewCheckM> args3 = new ConnectionEventArgs<WebViewCheckM>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs");
						}
						break;
					}
					catch (Exception ex2)
					{
						WebErrorLog.Instance.StartLog(ex2, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 238);
						break;
					}
				}
				case ViewCheckMActions.DeleteRequest:
				{
					if (this.OnDeletedRequest == null)
					{
						break;
					}
					ConnectionEventArgs<Guid> args = new ConnectionEventArgs<Guid>(pack, connectionUniqueId, connection);
					try
					{
						await Utilities.RaiseEvent(this.OnDeletedRequest, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs");
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 248);
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
				WebErrorLog.Instance.StartLog(ex4, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 255);
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
				if (this.OnSync != null)
				{
					this.OnSync = null;
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebViewCheckM.cs", 273);
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
