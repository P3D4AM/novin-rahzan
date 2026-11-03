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
using PacketParser.Entities.Interfaces.Accounting;
using PacketParser.Entities.MetaData;
using PacketParser.Entities.SerializeData;
using PacketParser.EventArgs;

namespace PacketParser.Entities;

public class WebTafsilGroup : Serializable<WebTafsilGroup>, ITafsilGroup, IHasGuid, IHasModified, IHasStatus, IHasJustGuid, INpDisposable, IDisposable, ISyncFromClient2Server<WebTafsilGroup>, IDeliverableEntity, IEditableDelivery, IReceiver
{
	public Guid Guid { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public string Tbl { get; set; }

	public long CodeMin { get; set; }

	public long CodeMax { get; set; }

	public bool Checked { get; set; }

	public bool Status { get; set; }

	public Guid MoeinGuid { get; set; }

	public Guid MoeinSellGuid { get; set; }

	public Guid MoeinBuyGuid { get; set; }

	public Guid MoeinBackSellGuid { get; set; }

	public Guid MoeinBackBuyGuid { get; set; }

	public Guid MoeinReceptionGuid { get; set; }

	public Guid MoeinPardakhtGuid { get; set; }

	public Guid MoeinAmaniMaNazdDigaranGuid { get; set; }

	public Guid MoeinAmaniDigaranNazdMaGuid { get; set; }

	[DefaultValue(EnTafsilGroupType.Floating)]
	public EnTafsilGroupType Type { get; set; }

	public short Id { get; set; }

	public DefualtBazaryabGuidInTafsilGroup BazaryabDefaultGuid { get; set; }

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

	public TafsilGroupMetaData MetaData { get; set; }

	public event Func<object, TafsilLevelReadEventArgs, Task> OnReadRequest;

	public event Func<object, ConnectionEventArgs<WebTafsilGroup>, Task> OnReadResponse;

	public event Func<object, ConnectionEventArgs<WebTafsilGroup>, Task> OnSaveRequest;

	public event Func<object, SaveResponseEventArgs, Task> OnSaveResponse;

	public event Func<object, ConnectionEventArgs<WebTafsilGroup>, Task> OnSync;

	public static Packet GenerateReadRequest(int requestedUId, DateTime modified)
	{
		using (new JaegerMonitor("GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)5).ToString() };
				TafsilLevelReadEventArgs tafsilLevelReadEventArgs = new TafsilLevelReadEventArgs
				{
					Modified = modified
				};
				list.Add(tafsilLevelReadEventArgs.ToString());
				return new Packet(PacketType.TafsilGroup, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 79);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReadResponse(IEnumerable<WebTafsilGroup> items, bool isSync = false)
	{
		if (items == null || items.Count() <= 0)
		{
			throw new NullReferenceException();
		}
		using (new JaegerMonitor("GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs"))
		{
			try
			{
				return items?.GeneratePackets(PacketType.TafsilGroup, 6, null, isSync, null, 0, deliveryRequst: false, "GenerateReadResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReadResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 92);
				return null;
			}
		}
	}

	public static List<Packet> GenerateSaveRequest(IEnumerable<WebTafsilGroup> levels, int partitionId = 0)
	{
		using (new JaegerMonitor("GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs"))
		{
			try
			{
				object result;
				if (levels == null)
				{
					result = null;
				}
				else
				{
					result = levels.GeneratePackets(PacketType.TafsilGroup, 1, null, isSync: false, null, partitionId, deliveryRequst: false, "GenerateSaveRequest");
				}
				return (List<Packet>)result;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 106);
				throw;
			}
		}
	}

	public static List<Packet> GenerateSaveResponse(IEnumerable<SaveResponse> responses)
	{
		using (new JaegerMonitor("GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs"))
		{
			try
			{
				return responses?.GeneratePackets(PacketType.TafsilGroup, 2, null, isSync: false, null, 0, deliveryRequst: false, "GenerateSaveResponse");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSaveResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 120);
				throw;
			}
		}
	}

	public static List<Packet> GenerateReset(IEnumerable<WebTafsilGroup> levels)
	{
		using (new JaegerMonitor("GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs"))
		{
			try
			{
				return levels.GeneratePackets(PacketType.TafsilGroup, 7, null, isSync: false, null, 0, deliveryRequst: true, "GenerateReset");
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 134);
				return null;
			}
		}
	}

	public async Task DataReceived(Packet pack, string connectionUniqueId, ConnectionData connection)
	{
		if (IsDisposed)
		{
			throw new ObjectDisposedException(GetType().Name);
		}
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs"))
		{
			_ = 4;
			try
			{
				switch ((TafsilLevelActions)pack.PacketActions)
				{
				case TafsilLevelActions.SaveRequest:
					if (this.OnSaveRequest != null)
					{
						ConnectionEventArgs<WebTafsilGroup> args3 = new ConnectionEventArgs<WebTafsilGroup>(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveRequest, this, args3, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs");
					}
					break;
				case TafsilLevelActions.SaveResponse:
					if (this.OnSaveResponse != null)
					{
						SaveResponseEventArgs args2 = new SaveResponseEventArgs(pack, connectionUniqueId, connection);
						await Utilities.RaiseEvent(this.OnSaveResponse, this, args2, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs");
					}
					break;
				case TafsilLevelActions.ReadRequest:
					if (this.OnReadRequest != null)
					{
						TafsilLevelReadEventArgs args4 = new TafsilLevelReadEventArgs
						{
							ConnectionUniqueId = connectionUniqueId,
							Connection = connection
						};
						await Utilities.RaiseEvent(this.OnReadRequest, this, args4, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs");
					}
					break;
				case TafsilLevelActions.ReadResponse:
				{
					if (this.OnReadResponse == null && this.OnSync == null)
					{
						break;
					}
					ConnectionEventArgs<WebTafsilGroup> args = new ConnectionEventArgs<WebTafsilGroup>(pack, connectionUniqueId, connection);
					try
					{
						if (pack.IsSynced)
						{
							await Utilities.RaiseEvent(this.OnSync, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs");
						}
						else
						{
							await Utilities.RaiseEvent(this.OnReadResponse, this, args, connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs");
						}
						break;
					}
					catch (Exception ex)
					{
						WebErrorLog.Instance.StartLog(ex, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 177);
						break;
					}
				}
				case (TafsilLevelActions)3:
				case (TafsilLevelActions)4:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex3)
			{
				WebErrorLog.Instance.StartLog(ex3, "", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 184);
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			TafsilLevelActions tafsilLevelActions = (TafsilLevelActions)action;
			return tafsilLevelActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 195);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((TafsilLevelActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 208);
			return $"Error({action})";
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
			WebErrorLog.Instance.StartLog(ex, "", "Dispose", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\WebTafsilGroup.cs", 226);
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
