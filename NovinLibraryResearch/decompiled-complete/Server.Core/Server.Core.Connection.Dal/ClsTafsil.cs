using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsTafsil : IEntity<WebTafsil>
{
	private class NestedTafsil
	{
		internal static readonly ClsTafsil TafsilInstance;

		static NestedTafsil()
		{
			TafsilInstance = new ClsTafsil();
		}
	}

	public static ClsTafsil TafsilInstance => NestedTafsil.TafsilInstance;

	private ClsTafsil()
	{
	}

	public async Task<List<WebTafsil>> ApplyAccessAsync(IEnumerable<WebTafsil> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return null;
				}
				if (connection.Source != ENSource.Accounting)
				{
					lst?.Count();
					lst = lst?.Where((WebTafsil p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return lst?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 40);
				return null;
			}
		}
	}

	internal async Task TafsilInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTafsil> e)
	{
		using (new JaegerMonitor("TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Tafsil, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebTafsil> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				line = 3;
				bool result = true;
				try
				{
					line = 4;
					ret = await Server.Core.Assistance.Initializer.Dal.Tafsil.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					line = 5;
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
					line = 6;
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 81);
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				line = 150;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					line = 151;
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 152;
					if (!(connection2?.IsDisposed ?? true))
					{
						await connection2.SendPacketAsync(WebTafsil.GenerateSaveResponse(ret), Priority.High);
					}
				}
				line = 16;
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				line = 17;
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				line = 18;
				IEnumerable<WebTafsil> truSavedInRet = e.ConnectionData.Where((WebTafsil p) => trueSaved.Contains(p.Guid));
				line = 19;
				if (truSavedInRet == null)
				{
					return;
				}
				if (truSavedInRet != null && truSavedInRet.Count() <= 0)
				{
					return;
				}
				line = 20;
				ConnectionEventArgs<WebTafsil> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					line = 21;
					ClsConnectionData accountingConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accountingConnection != null && accountingConnection?.ClientVersion >= 10)
					{
						accountingConnection?.SendPacketAsync(WebTafsil.GenerateSaveRequest(e.ConnectionData, 0), Priority.High);
					}
				}
				line = 23;
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.ClientVersion >= 10 && p.IsAccountingFamily && p.Source != ENSource.Accounting);
				line = 25;
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData connection in enumerable)
				{
					if (connection?.IsDisposed ?? true)
					{
						continue;
					}
					List<WebTafsil> lst = await ApplyAccessAsync(truSavedInRet, connection);
					if (lst?.Any() ?? false)
					{
						Task.Run(() => connection.SendPacketAsync(WebTafsil.GenerateReadResponse(lst, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"Line:{line}", "TafsilInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 147);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		int line = 0;
		try
		{
			if (!connection.IsAccountingFamily || connection.ClientVersion < 10)
			{
				return ret;
			}
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 159);
			}
			ClsConnectionData clsConnectionData = connection;
			if (clsConnectionData != null && clsConnectionData.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			SyncInformation obj = await connection.GetSyncInfoAsync();
			line = 1;
			if (obj == null)
			{
				ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 165);
				return ret;
			}
			if (!(await connection.GetDepartmentAsync()).SyncsKolMoeinTafsil)
			{
				return ret.AddInformation("این دپارتمان حساب تفصیل را سینک نمیکند", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 171);
			}
			line = 2;
			line = 21;
			List<short> totalPartitions = new List<short>();
			List<short> list = await Server.Core.Assistance.Initializer.Dal.Tafsil.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (list != null && list.Any())
			{
				totalPartitions.AddRange(list);
				while (list != null && list.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 219);
						return ret;
					}
					await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 225);
						return ret;
					}
					line = 22;
					if (connection == null || connection.IsDisposed)
					{
						return ret;
					}
					list = await Server.Core.Assistance.Initializer.Dal.Tafsil.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
					line = 23;
					list = list?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
					line = 24;
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 237);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 240);
			ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 241);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			sb.AppendLine($"TafsilSyncResult:{!ret.HasError}");
		}
		return ret;
	}

	private async Task<bool> SyncPartitionsAsync(List<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		_ = 1;
		try
		{
			new CancellationTokenSource();
			if (partitions != null)
			{
				if (partitions == null || partitions.Count > 0)
				{
					List<Task> tasks = new List<Task>();
					foreach (short partition in partitions)
					{
						tasks.Add(syncPartitionAsync(partition, connection));
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 268);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> syncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			line = 25;
			DateTime syncing = DateTime.Now;
			line = 26;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 282);
			}
			bool loadGardeshHesab = connection.ClientVersion <= 14 && (connection.Source == ENSource.Android_HesabYar || connection.Source == ENSource.Baje || connection.Source == ENSource.AccountingPortal);
			IEnumerable<WebTafsil> lst = await Server.Core.Assistance.Initializer.Dal.Tafsil.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken, loadGardeshHesab);
			line = 30;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 292);
			}
			if (lst?.Any() ?? false)
			{
				using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Tafsil, 300000, connection.cancellationToken);
				line = 40;
				Task.Run(() => connection.SendPacketAsync(WebTafsil.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					ret.AddReturnedValue(ReturnedState.Error, "waiter time out", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 307);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 310);
				}
				line = 45;
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Tafsil, syncing, connection), ReturnedState.None, "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 312);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 315);
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 318);
			ret.AddReturnedValue(ex2, "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 319);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendTafsilAsync(WebTafsil tafsil, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.ClientVersion < 10)
				{
					return ret.AddInformation("clientVerison is less than 10", "", "SendTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 330);
				}
				using TafsilWaiter waiter = new TafsilWaiter(connection.Context, tafsil.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebTafsil.GenerateSaveRequest(new List<WebTafsil> { tafsil }, 0), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ClsTafsil.SendTafsilAsync"), ReturnedState.None, "SendTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 333);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 335);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 338);
				ret.AddReturnedValue(ex, "", "SendTafsilAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 339);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, bool IsGardeshHesab, short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				if (connection.ClientVersion < 10)
				{
					return ret.AddInformation("Client version is less than 10 so sync tafsil is diable!", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 352);
				}
				line = 2;
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Source({connection.Source})!=Accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 354);
				}
				line = 3;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Tafsil, connection.cancellationToken);
				line = 4;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 358);
				}
				line = 5;
				List<Task> tasks = new List<Task>();
				line = 6;
				if (list != null && list.Any())
				{
					foreach (Guid item2 in list)
					{
						line = 7;
						ret.AddInformation($"sending tafsil({item2}) to accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 366);
						line = 8;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 368);
						}
						line = 9;
						WebTafsil webTafsil = await Server.Core.Assistance.Initializer.Dal.Tafsil.GetAsync(item2, connection.DepId, IsGardeshHesab, connection.cancellationToken);
						line = 10;
						if (webTafsil != null)
						{
							line = 11;
							tasks.Add(connection.SendPacketAsync(WebTafsil.GenerateSaveRequest(new List<WebTafsil> { webTafsil }, 0), Priority.Medium));
							line = 12;
							if (tasks.Count > maxParalelTasks)
							{
								line = 13;
								Task item = await Task.WhenAny(tasks);
								line = 14;
								tasks.Remove(item);
								line = 15;
							}
							line = 16;
						}
					}
				}
				line = 17;
				await Task.WhenAll(tasks);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 391);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 396);
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 397);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> SendTafsilIfNotDeliveredToAccAsync(ClsConnectionData accountingConnection, short maxParalelTasks, List<Guid?> guids, CancellationToken timeOutToken)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			line = 1;
			if (!(guids?.Any() ?? false))
			{
				return ret;
			}
			line = 2;
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (Guid? guid in guids)
			{
				if (!((guid ?? Guid.Empty) != Guid.Empty))
				{
					continue;
				}
				line = 30;
				if (accountingConnection?.IsDisposed ?? true)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendTafsilIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 416);
				}
				line = 31;
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				WebTafsil webTafsil = await Server.Core.Assistance.Initializer.Dal.Tafsil.GetAsync(guid.Value, accountingConnection.DepId, loadGardeshHesab: false, accountingConnection.cancellationToken);
				line = 4;
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				if (webTafsil == null)
				{
					continue;
				}
				line = 5;
				if (webTafsil.ServerStatus == ServerStatusForClient.Delivered || webTafsil.ServerStatus == ServerStatusForClient.DirectDelivery)
				{
					continue;
				}
				if (timeOutToken.IsCancellationRequested)
				{
					return ret;
				}
				line = 6;
				tasks.Add(TafsilInstance.SendTafsilAsync(webTafsil, accountingConnection));
				line = 7;
				if (tasks.Count >= maxParalelTasks)
				{
					line = 8;
					Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
					if (timeOutToken.IsCancellationRequested)
					{
						return ret;
					}
					line = 9;
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SendTafsilIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 437);
					line = 10;
					tasks.Remove(t);
					line = 11;
				}
			}
			line = 15;
			if (timeOutToken.IsCancellationRequested)
			{
				return ret;
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendTafsilIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 446);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SendTafsilIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 450);
			ret.AddReturnedValue(ex, "", "SendTafsilIfNotDeliveredToAccAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsTafsil.cs", 451);
		}
		return ret;
	}
}
