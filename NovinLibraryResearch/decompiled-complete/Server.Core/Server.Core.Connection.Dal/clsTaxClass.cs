using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class clsTaxClass : IEntity<WebTax>
{
	private class NestedTax
	{
		internal static readonly clsTaxClass Instance;

		static NestedTax()
		{
			Instance = new clsTaxClass();
		}
	}

	public static clsTaxClass Instance => NestedTax.Instance;

	public Task<List<WebTax>> ApplyAccessAsync(IEnumerable<WebTax> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs"))
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
					lst = lst?.Where((WebTax p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 40);
				return null;
			}
		}
	}

	internal async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebTax> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Tax, 1, isReceivedPacket: true);
				if (!(e?.ConnectionData?.Any()).GetValueOrDefault())
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.TaxClass.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 62);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection2?.IsDisposed ?? true)
					{
						return;
					}
					await connection2.SendPacketAsync(WebTax.GenerateSaveResponse(ret), Priority.High);
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				List<WebTax> truSavedInRet = e.ConnectionData.Where((WebTax p) => trueSaved.Contains(p.Guid)).ToList();
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebTax> connectionEventArgs = e;
				if ((connectionEventArgs == null || connectionEventArgs.Connection.Source != ENSource.Accounting) && e.Connection.ClientVersion >= 17)
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 16)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebTax.GenerateSaveRequest(truSavedInRet, 0), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.Source != ENSource.Accounting && p.IsAccountingFamily && p.ClientVersion >= 17);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (WebTax item2 in truSavedInRet)
				{
					item2.DeliveryDate = DateTime.Now;
					item2.ServerStatus = ServerStatusForClient.Delivered;
				}
				foreach (ClsConnectionData connection in enumerable)
				{
					if (connection?.IsDisposed ?? true)
					{
						continue;
					}
					List<WebTax> dataForSend = await ApplyAccessAsync(truSavedInRet, connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => connection.SendPacketAsync(WebTax.GenerateReadResponse(dataForSend, isSync: false, e.PartitionId), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 108);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.ClientVersion <= 16 || (connection.Source != ENSource.Baje && connection.Source != ENSource.AccountingPortal && connection.Source != ENSource.Android_HesabYar))
		{
			return ret;
		}
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		try
		{
			if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 117);
			}
			if (await connection.GetSyncInfoAsync() == null)
			{
				ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 121);
				return ret;
			}
			List<short> totalPartitions = new List<short>();
			List<short> list = await Server.Core.Assistance.Initializer.Dal.TaxClass.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
			if (list != null && list.Any())
			{
				totalPartitions.AddRange(list);
				while (list != null && list.Any())
				{
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 136);
						return ret;
					}
					await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
					if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
					{
						ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 142);
						return ret;
					}
					if (connection == null || connection.IsDisposed)
					{
						return ret;
					}
					list = (await Server.Core.Assistance.Initializer.Dal.TaxClass.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
				}
			}
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 151);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 154);
			ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 155);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 180);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.ClientVersion < 17)
			{
				throw new Exception("version" + connection.Version + " is less than 17!");
			}
			DateTime syncing = DateTime.Now;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 190);
			}
			List<WebTax> lst = await Server.Core.Assistance.Initializer.Dal.TaxClass.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Tax, 300000, connection.cancellationToken);
			if (lst?.Any() ?? false)
			{
				Task.Run(() => connection.SendPacketAsync(WebTax.GenerateReadResponse(lst, isSync: true, partition), Priority.Low));
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					ret.AddReturnedValue(ReturnedState.Error, "waiter time out", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 199);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 202);
				}
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Tax, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 204);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 207);
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 211);
			ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 212);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection.cancellationToken, connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.Source != ENSource.Accounting || connection.ClientVersion <= 16)
				{
					return ret.AddInformation($"source({connection.Source}) is not accounting or version({connection.Version}) is less than 17", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 223);
				}
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Tax, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 225);
				}
				foreach (Guid item in list ?? new List<Guid>())
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 228);
					}
					WebTax webTax = await Server.Core.Assistance.Initializer.Dal.TaxClass.GetAsync(item, connection.DepId);
					if (webTax != null)
					{
						await connection.SendPacketAsync(WebTax.GenerateSaveRequest(new List<WebTax> { webTax }, 0), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 236);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 241);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 242);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection, short maxParallelTask = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 300000))
		{
			_ = 5;
			try
			{
				if (connection.ClientVersion <= 16)
				{
					return ret;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Tax, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 255);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				int timeOut = 180000;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid guid in list)
				{
					if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested || connection.DepId <= 0)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 260);
					}
					WebTax coupon = await Server.Core.Assistance.Initializer.Dal.TaxClass.GetAsync(guid, connection.DepId);
					if (coupon == null)
					{
						await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.DeleteAsync(connection.DepId, EntityPart.Tax, guid);
						if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested || connection.DepId <= 0)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 265);
						}
						continue;
					}
					tasks.Add(Task.Run(() => SendAsync(coupon, connection, timeOut, Priority.High)));
					if (tasks.Count > maxParallelTask)
					{
						Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
						returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 273);
						tasks.Remove(t);
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 277);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 281);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 286);
				WebErrorLog.Instance.StartLog(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 287);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendAsync(WebTax coupon, ClsConnectionData connection, int timeOutMiliSecond, Priority priority)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection?.IsDisposed ?? true)
			{
				return ret.AddErrorTokenIsCancelled("", "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 296);
			}
			if (ret.HasError || connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
			{
				return ret;
			}
			using DeliveryWaiter waiter = new DeliveryWaiter(connection.Context, coupon.Guid, EntityPart.Tax, connection.cancellationToken);
			List<Packet> pack = WebTax.GenerateSaveRequest(new List<WebTax> { coupon }, 0);
			Task.Run(() => connection.SendPacketAsync(pack, priority, 0, connection.cancellationToken));
			ret.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 305);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 309);
			ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsTaxClass.cs", 310);
		}
		return ret;
	}
}
