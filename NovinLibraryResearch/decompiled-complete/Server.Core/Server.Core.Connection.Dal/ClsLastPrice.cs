using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.CompositeKeys;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsLastPrice : IEntity<WebLastPrice>
{
	private static Lazy<ClsLastPrice> instance = new Lazy<ClsLastPrice>(isThreadSafe: true);

	public static ClsLastPrice Instance => instance.Value;

	public async Task<List<WebLastPrice>> ApplyAccessAsync(IEnumerable<WebLastPrice> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebLastPrice p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	public async Task LastPriceInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebLastPrice> e)
	{
		using (new JaegerMonitor("LastPriceInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.LastPrice, 1, isReceivedPacket: true);
				if (e.ConnectionData == null || e.Connection == null)
				{
					return;
				}
				if (e.Connection?.DepId <= 0)
				{
					throw new Exception($"DepId is not valid{e}");
				}
				List<SaveResponse<LastPriceCompositeKey>> list = await Server.Core.Assistance.Initializer.Dal.LastPriceRepository.SaveAllAsync(e.ConnectionData, (e?.Connection?.DepId).GetValueOrDefault(), e.Connection?.cancellationToken ?? default(CancellationToken), flush: true, 3, e.Connection.Source);
				foreach (SaveResponse<LastPriceCompositeKey> item in list)
				{
					item.PartitionId = e.PartitionId;
				}
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebLastPrice.GenerateSaveResponse(list, e.PacketReferenceId), Priority.Medium);
				List<WebLastPrice> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				List<ClsConnectionData> list2 = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && ((p != null && p.Source == ENSource.Android_HesabYar) || (p != null && p.Source == ENSource.IOS_HesabYar) || (p != null && p.Source == ENSource.Android_SefareshGir) || (p != null && p.Source == ENSource.Baje) || (p != null && p.Source == ENSource.AccountingPortal) || (p != null && p.Source == ENSource.WebsiteWordPress) || (p != null && p.Source == ENSource.WebSite))).ToList();
				if (list2 == null)
				{
					return;
				}
				foreach (ClsConnectionData Connection in list2)
				{
					if (Connection == null || Connection.IsDisposed || Connection.ClientVersion <= 7)
					{
						continue;
					}
					List<WebLastPrice> lst = await ApplyAccessAsync(e.ConnectionData, Connection);
					if (lst != null && lst.Any())
					{
						Task.Run(() => Connection.SendPacketAsync(WebLastPrice.GenerateReadResponse(lst, 0), Priority.Medium));
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "LastPriceInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 74);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		int line = 0;
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.ClientVersion < 8)
			{
				return ret;
			}
			if (connection.DepId == 908)
			{
				return ret;
			}
			line = 1;
			connection?.cancellationToken.ThrowIfCancellationRequested();
			line = 2;
			if (await connection.GetSyncInfoAsync() == null)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 89);
			}
			line = 21;
			List<short> totalPartitions = new List<short>();
			line = 22;
			IEnumerable<short> enumerable = await Server.Core.Assistance.Initializer.Dal.LastPriceRepository.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			line = 23;
			if (enumerable != null && enumerable.Any())
			{
				line = 24;
				totalPartitions.AddRange(enumerable);
				line = 25;
				while (enumerable != null && enumerable.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 104);
						return ret;
					}
					line = 26;
					await SyncPartitionsAsync(enumerable, connection, ConnectionUniqueId, maxParalel);
					line = 27;
					enumerable = await Server.Core.Assistance.Initializer.Dal.LastPriceRepository.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
					line = 28;
					enumerable = enumerable?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					line = 29;
				}
				line = 30;
			}
			line = 31;
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 119);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 122);
			ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 123);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}

	private async Task<bool> SyncPartitionsAsync(IEnumerable<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		_ = 1;
		try
		{
			new CancellationTokenSource();
			if (partitions != null)
			{
				if (partitions == null || partitions.Count() > 0)
				{
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
			}
			return true;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 146);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection?.IsDisposed ?? true)
			{
				ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 155);
				return ret;
			}
			DateTime syncing = DateTime.Now;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 157);
			}
			IEnumerable<WebLastPrice> lst = await Server.Core.Assistance.Initializer.Dal.LastPriceRepository.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			lst = await ApplyAccessAsync(lst, connection);
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 172);
			}
			if (lst?.Any() ?? false)
			{
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.LastPrice, 300000, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebLastPrice.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 179);
					return ret;
				}
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.LastPrice, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 183);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 186);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 189);
			ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsLastPrice.cs", 190);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}
}
