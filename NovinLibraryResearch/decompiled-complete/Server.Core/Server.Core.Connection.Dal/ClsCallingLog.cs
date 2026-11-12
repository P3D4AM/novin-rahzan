using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsCallingLog : IEntity<WebCallingLog>
{
	private class NestedCallingLog
	{
		internal static readonly ClsCallingLog CallingLogInstance;

		static NestedCallingLog()
		{
			CallingLogInstance = new ClsCallingLog();
		}
	}

	[CompilerGenerated]
	private sealed class _003C_003Ec__DisplayClass9_1
	{
		public List<short> totalPartitions;

		internal bool _003CSync2ClientAsync_003Eb__1(short x)
		{
			if (totalPartitions != null)
			{
				return !totalPartitions.Contains(x);
			}
			return false;
		}
	}

	private static ConcurrentDictionary<short, bool> dic_IsNewCallingLogExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsCallingLog CallingLogInstance => NestedCallingLog.CallingLogInstance;

	private ClsCallingLog()
	{
	}

	public async Task<List<WebCallingLog>> ApplyAccessAsync(IEnumerable<WebCallingLog> lst, IClsConnectionData connection)
	{
		return null;
	}

	public async Task<IEnumerable<WebCallingLog>> ApplyAccessAsync(List<WebCallingLog> lst, Guid token)
	{
		return null;
	}

	internal async Task CallingLogInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCallingLog> e)
	{
		using (new JaegerMonitor("CallingLogInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.CallingLog, 1, isReceivedPacket: true);
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					return;
				}
				line = 1;
				IEnumerable<SaveResponse> enumerable = await Server.Core.Assistance.Initializer.Dal.CallingLog.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
				foreach (SaveResponse item in enumerable)
				{
					item.PartitionId = e.PartitionId;
				}
				line = 2;
				Task.Run(() => SendToTelegramAsync(e));
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebCallingLog.GenerateSaveResponse(enumerable), Priority.High);
				}
				line = 4;
				ConnectionEventArgs<WebCallingLog> connectionEventArgs = e;
				if (connectionEventArgs == null || connectionEventArgs.Connection.Source != ENSource.Accounting)
				{
					ConnectionEventArgs<WebCallingLog> connectionEventArgs2 = e;
					if (connectionEventArgs2 != null && connectionEventArgs2.ConnectionData?.Count > 0)
					{
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebCallingLog.GenerateSaveRequest(e.ConnectionData, 0), Priority.Medium);
					}
				}
				line = 5;
				List<WebCallingLog> connectionData = e.ConnectionData;
				if (connectionData != null && connectionData.Count > 0)
				{
					List<ClsConnectionData> list = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting) && p?.ClientVersion >= 7).ToList();
					line = 6;
					if (list != null)
					{
						foreach (ClsConnectionData Connection in list)
						{
							if (Connection == null || Connection.IsDisposed)
							{
								continue;
							}
							List<WebCallingLog> sendList = await ApplyAccessAsync(e.ConnectionData, Connection);
							if (sendList?.Any() ?? false)
							{
								Task.Run(() => Connection.SendPacketAsync(WebCallingLog.GenerateReadResponse(sendList, 0), Priority.Medium));
							}
						}
					}
				}
				line = 7;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "Line=" + line, "CallingLogInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 107);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebCallingLog.GenerateSaveResponse(responses), Priority.High);
			}
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendToTelegramAsync(ConnectionEventArgs<WebCallingLog> e, CancellationToken token = default(CancellationToken))
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (e.Connection.DepId == 3 && e.ConnectionData != null)
			{
				foreach (WebCallingLog connectionDatum in e.ConnectionData)
				{
					WebTelegramMessage webTelegramMessage = await connectionDatum.ToWebTelegramAsync(e.Connection.DepId, token);
					if (!string.IsNullOrEmpty(webTelegramMessage?.Message))
					{
						webTelegramMessage.Sent2web("", null, "SendToTelegramAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 128);
					}
				}
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendToTelegramAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 133);
			ret.AddReturnedValue(ex, "", "SendToTelegramAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 134);
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string connectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				new _003C_003Ec__DisplayClass9_1();
				return ret;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 188);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 191);
				ret.AddReturnedValue(ex2, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 192);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}

	private async Task SyncWasReset(ClsConnectionData connection)
	{
		try
		{
			int num;
			if ((uint)num > 3u)
			{
				return;
			}
			try
			{
				IEnumerable<WebCallingLog> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.CallingLog.GetAllAsync(connection.DepId, connection.cancellationToken), connection);
				List<SyncResetItem> ResetItems = new List<SyncResetItem>();
				if ((enumerable?.Count() ?? 0) > 0)
				{
					foreach (WebCallingLog item in enumerable)
					{
						_ = item;
						ResetItems.Add(new SyncResetItem());
					}
				}
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.CallingLog, 300000, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.CallingLog, ResetItems), Priority.High));
				if (await waiter.WaitForSyncDonePacket())
				{
					await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.CallingLog, connection.cancellationToken);
				}
			}
			finally
			{
				JaegerMonitor monitor = default(JaegerMonitor);
				((IDisposable)monitor)?.Dispose();
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncWasReset", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 221);
		}
	}

	private async Task<bool> SyncPartitionsAsync(IEnumerable<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		_ = 1;
		try
		{
			if ((partitions?.Count() ?? 0) <= 0)
			{
				return true;
			}
			List<Task> tasks = new List<Task>();
			foreach (short partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					List<Task> list = tasks;
					list.Remove(await Task.WhenAny(tasks));
				}
			}
			await Task.WhenAll(tasks);
			return true;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 241);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				return ret;
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 337);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 340);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 341);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				return ret;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 408);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 411);
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCallingLog.cs", 412);
			}
			return ret;
		}
	}
}
