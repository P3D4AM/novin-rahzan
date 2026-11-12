using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using PacketParser.TransferLayer.Waiters.Accounting.Product;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsProductVahed : IEntity<WebPrdVahed>
{
	private class NestedProductVahed
	{
		internal static readonly ClsProductVahed ProductVahedInstance;

		static NestedProductVahed()
		{
			ProductVahedInstance = new ClsProductVahed();
		}
	}

	public static ClsProductVahed ProductVahedInstance => NestedProductVahed.ProductVahedInstance;

	private ClsProductVahed()
	{
	}

	public async Task<List<WebPrdVahed>> ApplyAccessAsync(IEnumerable<WebPrdVahed> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebPrdVahed p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	public async Task<IEnumerable<WebPrdVahed>> ApplyAccessAsync(IEnumerable<WebPrdVahed> lst, Guid token)
	{
		return lst;
	}

	public async Task<ReturnedSaveFuncInfo> SendVahedAsync(WebPrdVahed vahed, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using PrdVahedWaiter waiter = new PrdVahedWaiter(connection.Context, vahed.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebPrdVahed.GenerateSaveRequest(new List<WebPrdVahed> { vahed }), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ClsProductVahed.SendVahedAsync"), ReturnedState.None, "SendVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 55);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 57);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 60);
				ret.AddReturnedValue(ex, "", "SendVahedAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 61);
			}
			return ret;
		}
	}

	internal async Task PrdVahedInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdVahed> e)
	{
		using (new JaegerMonitor("PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ProductVahed, 1, isReceivedPacket: true);
				ConnectionEventArgs<WebPrdVahed> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.ProductVahed.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					WebErrorLog.Instance.StartLog(ex2, "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 86);
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebPrdVahed.GenerateSaveResponse(ret), Priority.High);
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebPrdVahed> truSavedInRet = e.ConnectionData.Where((WebPrdVahed p) => trueSaved.Contains(p.Guid));
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebPrdVahed> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebPrdVahed.GenerateSaveRequest(truSavedInRet), Priority.Medium));
					}
				}
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				List<ClsConnectionData> list = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting) && p?.ClientVersion > 8).ToList();
				if (list == null)
				{
					return;
				}
				foreach (ClsConnectionData Connection in list)
				{
					if (Connection != null && !Connection.IsDisposed)
					{
						Task.Run(async delegate
						{
							ClsConnectionData clsConnectionData = Connection;
							return clsConnectionData.SendPacketAsync(WebPrdVahed.GenerateReadResponse(await ApplyAccessAsync(truSavedInRet, Connection)), Priority.Medium);
						});
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "PrdVahedInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 127);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 3000000))
		{
			int line = 1;
			try
			{
				if (connection?.cancellationToken.IsCancellationRequested ?? true)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 138);
				}
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				line = 3;
				DateTime syncing = DateTime.Now;
				line = 4;
				IEnumerable<WebPrdVahed> lst = await Server.Core.Assistance.Initializer.Dal.ProductVahed.GetAllAsync(connection.DepId, syncInformation?.ProductVahed ?? DateTime.Now.AddYears(-20), connection.cancellationToken);
				line = 5;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 146);
				}
				line = 6;
				line = 8;
				lst = await ApplyAccessAsync(lst, connection);
				line = 9;
				line = 11;
				if (lst?.Any() ?? false)
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.ProductVahed, 300000, connection.cancellationToken);
					line = 12;
					Task.Run(() => connection.SendPacketAsync(WebPrdVahed.GenerateReadResponse(lst, isSync: true), Priority.Low));
					line = 13;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "waiter.WaitForSyncDonePacket", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 164);
						return ret;
					}
					line = 14;
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.ProductVahed, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 169);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 172);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 175);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 176);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, short maxParallelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs"))
		{
			_ = 4;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"source({connection.Source}) is != accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 188);
				}
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.ProductVahed, connection.cancellationToken);
				if (list == null || !list.Any())
				{
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 192);
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					ret.AddInformation($"Sending prdVahed({item}) to accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 196);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 197);
					}
					WebPrdVahed webPrdVahed = await Server.Core.Assistance.Initializer.Dal.ProductVahed.GetAsync(connection.DepId, item);
					if (webPrdVahed != null)
					{
						tasks.Add(SendVahedAsync(webPrdVahed, connection));
						if (tasks.Count >= maxParallelTasks)
						{
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
							tasks.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 205);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 208);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 212);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 217);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductVahed.cs", 218);
			}
			return ret;
		}
	}
}
