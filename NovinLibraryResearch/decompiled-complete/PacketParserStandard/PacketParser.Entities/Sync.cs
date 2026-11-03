using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using PacketParser.BusinessLayer;
using PacketParser.EventArgs;
using PacketParser.Mapping;

namespace PacketParser.Entities;

public class Sync : IReceiver, IItemAsyncMapper<Sync, Sync>
{
	private SyncInformation _SyncInfo = new SyncInformation();

	private DateTime? _LastSync;

	private ConcurrentDictionary<Guid, DateTime> _guids;

	private CancellationTokenSource _token;

	public Guid Token { get; set; }

	public long HashId { get; set; }

	public short DepId { get; set; }

	public int UserId { get; set; }

	public long Lk { get; set; }

	public long Ime { get; set; }

	public Guid DbGuid { get; set; }

	public DateTime InsertTime { get; set; }

	public DateTime LastSync
	{
		get
		{
			if (!_LastSync.HasValue)
			{
				_LastSync = DateTime.Now.AddDays(-1000.0);
			}
			return _LastSync.Value;
		}
		set
		{
			_LastSync = value;
		}
	}

	public SyncInformation SyncInfo
	{
		get
		{
			return _SyncInfo ?? (_SyncInfo = new SyncInformation());
		}
		set
		{
			_SyncInfo = value;
		}
	}

	public List<SyncLog> ListDets { get; set; }

	[JsonIgnore]
	[XmlIgnore]
	public ConcurrentDictionary<Guid, DateTime> Guids => _guids ?? (_guids = new ConcurrentDictionary<Guid, DateTime>());

	public byte[] HashData { get; set; }

	public bool IsSignalROnline { get; set; }

	[DefaultValue(4)]
	public LogLevel MinLogLevel { get; set; } = LogLevel.Error;


	public Guid CashierGuid { get; set; }

	public event Func<object, SyncDoneEventArgs, Task> OnSyncDone;

	public event Func<object, DeleteResponseEventArgs, Task> OnDeleteResponse;

	public event Func<object, SyncResetEventArgs, Task> OnSyncReset;

	public event Func<object, SyncResetDoneEventArgs, Task> OnSyncResetDone;

	public event Func<object, SyncDoneResponseEventArgs, Task> OnSyncDoneResponse;

	public static Packet GenerateSyncResetDone(EntityPart syncPart)
	{
		using (new JaegerMonitor("GenerateSyncResetDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)1).ToString() };
				SyncResetDoneEventArgs syncResetDoneEventArgs = new SyncResetDoneEventArgs
				{
					SyncPart = syncPart
				};
				list.Add(syncResetDoneEventArgs.ToString());
				return new Packet(PacketType.Sync, 5, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSyncResetDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 97);
				return null;
			}
		}
	}

	public static Packet GenerateConnectionStateChanged(Sync sync, EnConnectionState previous, EnConnectionState next)
	{
		using (new JaegerMonitor("GenerateConnectionStateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			try
			{
				if (sync == null)
				{
					throw new ArgumentNullException("sync");
				}
				List<string> list = new List<string> { ((short)1).ToString() };
				SyncStateChangedEventArgs syncStateChangedEventArgs = new SyncStateChangedEventArgs
				{
					PreviousState = previous,
					NextState = next
				};
				list.Add(syncStateChangedEventArgs.ToString());
				return new Packet(PacketType.Sync, 1, Json.ToBytesJson(sync), list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateConnectionStateChanged", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 124);
				return null;
			}
		}
	}

	public static Packet GenerateSyncDone(EntityPart syncPart, int partitionId, ReturnedSaveFuncInfo result)
	{
		using (new JaegerMonitor("GenerateSyncDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			ReturnedSaveFuncInfo returnedSaveFuncInfo = new ReturnedSaveFuncInfo();
			try
			{
				List<string> list = new List<string> { ((short)2).ToString() };
				SyncDoneEventArgs syncDoneEventArgs = new SyncDoneEventArgs
				{
					Part = 0,
					TotalParts = 1,
					PacketReferenceId = Guid.NewGuid(),
					SyncPart = syncPart,
					PartitionId = partitionId,
					Message = result.ErrorMessage,
					Result = !result.HasError
				};
				list.Add(syncDoneEventArgs.ToString());
				string extraDataForDebug = $"SyncDone:{syncPart}/partitionId:{partitionId}";
				return new Packet(PacketType.Sync, 2, null, list.ToArray(), 0, 1, Guid.NewGuid(), isSynced: false, 0, extraDataForDebug)
				{
					DeliveryRequested = false
				};
			}
			catch (Exception ex)
			{
				returnedSaveFuncInfo.AddReturnedValue(ex, "", "GenerateSyncDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 158);
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSyncDone", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 159);
				return null;
			}
		}
	}

	public static Packet GenerateDeleteResponse(EntityPart syncPart, int partitionId)
	{
		using (new JaegerMonitor("GenerateDeleteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			try
			{
				List<string> list = new List<string> { ((short)7).ToString() };
				SyncDoneEventArgs syncDoneEventArgs = new SyncDoneEventArgs
				{
					Part = 0,
					TotalParts = 1,
					PacketReferenceId = Guid.Empty,
					SyncPart = syncPart,
					PartitionId = partitionId
				};
				list.Add(syncDoneEventArgs.ToString());
				return new Packet(PacketType.Sync, 7, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateDeleteResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 186);
				return null;
			}
		}
	}

	public static Packet GenerateSyncReset(EntityPart part, List<SyncResetItem> items)
	{
		using (new JaegerMonitor("GenerateSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			try
			{
				Guid packetReferenceId = Guid.NewGuid();
				List<string> list = new List<string> { ((short)3).ToString() };
				SyncResetEventArgs syncResetEventArgs = new SyncResetEventArgs
				{
					Part = 0,
					TotalParts = 1,
					PacketReferenceId = packetReferenceId,
					EntityPart = part,
					SyncResetItems = items
				};
				list.Add(syncResetEventArgs.ToString());
				return new Packet(PacketType.Sync, 3, null, list.ToArray(), 0, 1, Guid.NewGuid());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSyncReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 214);
				throw;
			}
		}
	}

	public static Packet GenerateSyncDoneResponse(SyncInformation Syncinfo)
	{
		using (new JaegerMonitor("GenerateSyncDoneResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			try
			{
				Guid packetReferenceId = Guid.NewGuid();
				List<string> list = new List<string> { ((short)6).ToString() };
				SyncDoneResponseEventArgs syncDoneResponseEventArgs = new SyncDoneResponseEventArgs
				{
					Part = 0,
					TotalParts = 1,
					PacketReferenceId = packetReferenceId,
					SyncInfo = Syncinfo
				};
				list.Add(syncDoneResponseEventArgs.ToString());
				string extraDataForDebug = "SyncDoneResponse";
				return new Packet(PacketType.Sync, 6, null, list.ToArray(), 0, 1, Guid.Empty, isSynced: false, 0, extraDataForDebug)
				{
					DeliveryRequested = false
				};
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "GenerateSyncDoneResponse", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 252);
				throw;
			}
		}
	}

	public string GetPacketActionEnumName(short action)
	{
		try
		{
			SyncActions syncActions = (SyncActions)action;
			return syncActions.ToString();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionEnumName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 266);
			return $"Error({action})";
		}
	}

	public string GetPacketActionName(short action)
	{
		try
		{
			return ((SyncActions)action).GetDisplay();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetPacketActionName", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 279);
			return $"Error({action})";
		}
	}

	public async Task DataReceived(Packet pack, string ConnectionUniqueId, ConnectionData Connection)
	{
		int line = 1;
		using (new JaegerMonitor("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs"))
		{
			_ = 3;
			try
			{
				switch ((SyncActions)pack.PacketActions)
				{
				case SyncActions.Done:
					line = 100;
					if (this.OnSyncDone != null)
					{
						line = 101;
						SyncDoneEventArgs syncDoneEventArgs = pack.Args[1].FromJson<SyncDoneEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 307);
						if (syncDoneEventArgs == null)
						{
							syncDoneEventArgs = new SyncDoneEventArgs();
						}
						line = 102;
						syncDoneEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						line = 103;
						syncDoneEventArgs.Connection = Connection;
						syncDoneEventArgs.Part = pack.Part;
						syncDoneEventArgs.TotalParts = pack.TotalParts;
						syncDoneEventArgs.PacketReferenceId = pack.PacketReferenceId;
						await Utilities.RaiseEvent(this.OnSyncDone, this, syncDoneEventArgs, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs");
					}
					break;
				case SyncActions.Reset:
					line = 200;
					if (this.OnSyncReset != null)
					{
						SyncResetEventArgs syncResetEventArgs = pack.Args[1].FromJson<SyncResetEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 321);
						syncResetEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						syncResetEventArgs.Connection = Connection;
						syncResetEventArgs.Part = pack.Part;
						syncResetEventArgs.TotalParts = pack.TotalParts;
						syncResetEventArgs.PacketReferenceId = pack.PacketReferenceId;
						await Utilities.RaiseEvent(this.OnSyncReset, this, syncResetEventArgs, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs");
					}
					break;
				case SyncActions.ResetDone:
					line = 300;
					if (this.OnSyncResetDone != null)
					{
						SyncResetDoneEventArgs syncResetDoneEventArgs = pack.Args[1].FromJson<SyncResetDoneEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 332);
						syncResetDoneEventArgs.ConnectionUniqueId = ConnectionUniqueId;
						syncResetDoneEventArgs.Connection = Connection;
						syncResetDoneEventArgs.TotalParts = pack.TotalParts;
						syncResetDoneEventArgs.PacketReferenceId = pack.PacketReferenceId;
						syncResetDoneEventArgs.Part = pack.Part;
						await Utilities.RaiseEvent(this.OnSyncResetDone, this, syncResetDoneEventArgs, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs");
					}
					break;
				case SyncActions.SyncDoneResponse:
				{
					line = 400;
					SyncDoneResponseEventArgs syncDoneResponseEventArgs = pack.Args[1].FromJson<SyncDoneResponseEventArgs>("DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 346);
					syncDoneResponseEventArgs.ConnectionUniqueId = ConnectionUniqueId;
					syncDoneResponseEventArgs.Connection = Connection;
					syncDoneResponseEventArgs.Part = pack.Part;
					syncDoneResponseEventArgs.TotalParts = pack.TotalParts;
					syncDoneResponseEventArgs.PacketReferenceId = pack.PacketReferenceId;
					await Utilities.RaiseEvent(this.OnSyncDoneResponse, this, syncDoneResponseEventArgs, Connection.cancellationToken, 60, "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs");
					break;
				}
				case SyncActions.StateChange:
				case SyncActions.ClientSynced:
					break;
				}
			}
			catch (OperationCanceledException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "DataReceived", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 359);
			}
		}
	}

	internal void Dispose()
	{
	}

	public async Task<Sync> MapAsync(string contextGuid, Sync source, Sync destination, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (source == null)
			{
				destination = null;
				return destination;
			}
			destination.Token = source.Token;
			destination.HashId = source.HashId;
			destination.DepId = source.DepId;
			destination.UserId = source.UserId;
			destination.CashierGuid = source.CashierGuid;
			destination.Lk = source.Lk;
			destination.Ime = source.Ime;
			destination.DbGuid = source.DbGuid;
			destination.InsertTime = source.InsertTime;
			destination.LastSync = source.LastSync;
			destination.HashData = source.HashData?.ToArray();
			destination.IsSignalROnline = source.IsSignalROnline;
			if (source.SyncInfo != null)
			{
				destination.SyncInfo = destination.SyncInfo ?? new SyncInformation();
				await SyncInfo.MapAsync(contextGuid, source.SyncInfo, destination.SyncInfo, tranName);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "MapAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 401);
		}
		return destination;
	}

	public Task<List<Sync>> MapListAsync(string contextGuid, IEnumerable<Sync> source, List<Sync> destination = null, string tranName = "", CancellationToken token = default(CancellationToken))
	{
		return source.MapListAsync(contextGuid, this, destination, tranName, 20);
	}

	public CancellationToken GetToken()
	{
		try
		{
			_token?.Cancel();
			_token = new CancellationTokenSource();
			return _token.Token;
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "GetToken", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\SharedPacketParser\\Entities\\Sync.cs", 432);
			return default(CancellationToken);
		}
	}
}
