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
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsMoein : IEntity<WebMoein>
{
	private class NestedMoein
	{
		internal static readonly ClsMoein MoeinInstance;

		static NestedMoein()
		{
			MoeinInstance = new ClsMoein();
		}
	}

	public static ClsMoein MoeinInstance => NestedMoein.MoeinInstance;

	private ClsMoein()
	{
	}

	public async Task<List<WebMoein>> ApplyAccessAsync(IEnumerable<WebMoein> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs"))
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
					lst = lst?.Where((WebMoein p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return lst?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 40);
				return null;
			}
		}
	}

	internal async Task MoeinInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebMoein> e)
	{
		using (new JaegerMonitor("MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Moein, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebMoein> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				List<SaveResponse> ret = new List<SaveResponse>();
				line = 3;
				bool result = true;
				try
				{
					line = 4;
					ret = await Server.Core.Assistance.Initializer.Dal.Moein.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 82);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				line = 15;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection2?.IsDisposed ?? true)
					{
						return;
					}
					await connection2.SendPacketAsync(WebMoein.GenerateSaveResponse(ret), Priority.High);
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (trueSaved == null)
				{
					return;
				}
				HashSet<Guid> hashSet = trueSaved;
				if (hashSet != null && hashSet.Count() <= 0)
				{
					return;
				}
				List<WebMoein> truSavedInRet = e.ConnectionData.Where((WebMoein p) => trueSaved.Contains(p.Guid)).ToList();
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebMoein> connectionEventArgs2 = e;
				if (connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting)
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebMoein.GenerateSaveRequest(truSavedInRet, 0), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.Source != ENSource.Accounting && p.IsAccountingFamily && p.ClientVersion >= 10);
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
					List<WebMoein> dataForSend = await ApplyAccessAsync(truSavedInRet, connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => connection.SendPacketAsync(WebMoein.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"Line:{line}", "MoeinInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 128);
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
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 140);
			}
			ClsConnectionData clsConnectionData = connection;
			if (clsConnectionData != null && clsConnectionData.Source == ENSource.WebsiteWordPress)
			{
				return ret;
			}
			SyncInformation obj = await connection.GetSyncInfoAsync();
			line = 1000;
			if (obj == null)
			{
				line = 1001;
				ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 147);
				line = 1002;
				return ret;
			}
			line = 1003;
			if (!(await connection.GetDepartmentAsync()).SyncsKolMoeinTafsil)
			{
				line = 1004;
				return ret.AddInformation($"({connection.DepName}) دپارتمان({connection.DepId}) حساب معین را سینک نمیکند", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 156);
			}
			line = 2;
			line = 21;
			List<short> totalPartitions = new List<short>();
			List<short> list = await Server.Core.Assistance.Initializer.Dal.Moein.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (list != null && list.Any())
			{
				totalPartitions.AddRange(list);
				while (list != null && list.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 207);
						return ret;
					}
					await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 213);
						return ret;
					}
					line = 22;
					if (connection == null || connection.IsDisposed)
					{
						return ret;
					}
					list = await Server.Core.Assistance.Initializer.Dal.Moein.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
					line = 23;
					list = list?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
					line = 24;
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 225);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 228);
			ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 229);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			sb.AppendLine($"MoeinSyncResult:{!ret.HasError}");
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 256);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> syncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		int line = 0;
		try
		{
			if (connection.ClientVersion < 10)
			{
				throw new Exception("version" + connection.Version + " is less than 10!");
			}
			line = 25;
			DateTime syncing = DateTime.Now;
			line = 26;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 271);
			}
			IEnumerable<WebMoein> lst = await Server.Core.Assistance.Initializer.Dal.Moein.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken, loadGardeshHesab: true);
			using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Moein, 300000, connection.cancellationToken);
			line = 30;
			if (lst?.Any() ?? false)
			{
				Task.Run(() => connection.SendPacketAsync(WebMoein.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					ret.AddReturnedValue(ReturnedState.Error, "waiter time out", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 295);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 298);
				}
				line = 45;
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Moein, syncing, connection), ReturnedState.None, "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 301);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 304);
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 308);
			ret.AddReturnedValue(ex2, "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 309);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection.cancellationToken, connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (connection.Source != ENSource.Accounting || connection.ClientVersion < 10)
				{
					return ret.AddInformation($"source({connection.Source}) is not accounting or version({connection.Version}) is less than 10", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 322);
				}
				line = 1;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Moein, connection.cancellationToken);
				line = 2;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 326);
				}
				line = 30;
				foreach (Guid item in list ?? new List<Guid>())
				{
					line = 4;
					ret.AddInformation($"در حال ارسال معین({item}) به حسابداری.", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 331);
					line = 5;
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 333);
					}
					line = 6;
					WebMoein webMoein = await Server.Core.Assistance.Initializer.Dal.Moein.GetAsync(item, connection.DepId, loadGardeshHesab: false);
					line = 7;
					if (webMoein != null)
					{
						line = 8;
						await connection.SendPacketAsync(WebMoein.GenerateSaveRequest(new List<WebMoein> { webMoein }, 0), Priority.Medium);
						line = 9;
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 345);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 350);
				WebErrorLog.Instance.StartLog(ex, $"#Line_{line}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsMoein.cs", 351);
			}
			return ret;
		}
	}
}
