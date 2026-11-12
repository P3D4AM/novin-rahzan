using System;
using System.Collections.Concurrent;
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

public class ClsDepot : IEntity<WebDepot>
{
	private class NestedDepot
	{
		internal static readonly ClsDepot DepotInstance;

		static NestedDepot()
		{
			DepotInstance = new ClsDepot();
		}
	}

	private static ConcurrentDictionary<short, DateTime> dic_isModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	public static ClsDepot DepotInstance => NestedDepot.DepotInstance;

	private ClsDepot()
	{
	}

	public async Task<List<WebDepot>> ApplyAccessAsync(IEnumerable<WebDepot> lst, IClsConnectionData connection)
	{
		int line = 0;
		try
		{
			if (!connection.IsAccountingFamily)
			{
				return null;
			}
			line = 1;
			if (connection.Source != ENSource.Accounting)
			{
				line = 2;
				lst?.Count();
				line = 3;
				lst = lst?.Where((WebDepot p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
			line = 4;
			if (lst != null)
			{
				IEnumerable<WebDepot> enumerable = lst;
				if (enumerable == null || enumerable.Count() > 0)
				{
					line = 5;
					if ((connection.Source == ENSource.Baje || connection.Source == ENSource.AccountingPortal || connection.Source == ENSource.CrmPortal) && connection.ClientVersion >= 10)
					{
						line = 6;
						return lst?.ToList();
					}
					line = 7;
					WebUserAcc userAccInfo = await connection.GetUserAccInfoAsync();
					if (userAccInfo.AccountingVersion < 2)
					{
						line = 8;
						WebUserAcc webUserAcc = userAccInfo;
						if (webUserAcc != null && !webUserAcc.AllDepotsAccess)
						{
							lst = lst.Where(delegate(WebDepot e)
							{
								Guid guid = e.Guid;
								Guid? obj = userAccInfo?.DepotGuid;
								return guid == obj;
							})?.ToList();
						}
					}
					line = 15;
					return lst?.ToList();
				}
			}
			return lst?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, $"#Line_{line}\r\n" + "Depots:\r\n" + Json.ToStringJson(lst), "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 78);
			return null;
		}
	}

	internal async Task DepotInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebDepot> e)
	{
		using (new JaegerMonitor("DepotInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Depot, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebDepot> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				List<SaveResponse> ret = new List<SaveResponse>();
				line = 3;
				bool result = true;
				line = 4;
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				await Server.Core.Assistance.Initializer.Dal.Depot.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, connection.Source, connection.cancellationToken);
				line = 5;
				ret.AddRange(e.ConnectionData.Select((WebDepot p) => new SaveResponse
				{
					SaveResultStatus = true,
					Guid = p.Guid,
					Date = DateTime.Now
				}));
				line = 6;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					connection?.SendPacketAsync(WebDepot.GenerateSaveResponse(ret), Priority.High);
				}
				line = 10;
				ConnectionEventArgs<WebDepot> connectionEventArgs2 = e;
				if (connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				List<WebDepot> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				line = 15;
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId);
				if (enumerable == null)
				{
					return;
				}
				foreach (ClsConnectionData Connection in enumerable?.Where((ClsConnectionData p) => p != null && p.Source != ENSource.Accounting && p.Source != ENSource.WebsiteWordPress))
				{
					List<WebDepot> depots = await ApplyAccessAsync(e?.ConnectionData, Connection);
					if (depots?.Any() ?? false)
					{
						Task.Run(() => Connection.SendPacketAsync(WebDepot.GenerateReadResponse(depots), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "DepotInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 144);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs"))
		{
			int line = 0;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Connection({connection.Source}) != Accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 154);
				}
				line = 1;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Depot, connection.cancellationToken);
				line = 2;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 158);
				}
				line = 3;
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				line = 4;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				if (list != null && list.Any())
				{
					foreach (Guid item in list)
					{
						line = 5;
						ret.AddInformation($"Sending depot({item}) to accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 166);
						line = 6;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 168);
						}
						line = 7;
						WebDepot webDepot = await Server.Core.Assistance.Initializer.Dal.Depot.LoadDataAsync(connection.DepId, item);
						line = 8;
						if (webDepot != null)
						{
							line = 9;
							tasks.Add(SendDepotAsync(webDepot, connection));
							if (tasks.Count >= maxParalelTasks)
							{
								line = 10;
								Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
								line = 11;
								tasks.Remove(task);
								line = 12;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 182);
							}
							line = 13;
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 186);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 190);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 195);
				WebErrorLog.Instance.StartLog(ex, "#ServerHub\r\n" + $"#Line_{line}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 196);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendDepotAsync(WebDepot depot, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.ClientVersion < 23)
				{
					depot.ServerStatus = ServerStatusForClient.Delivered;
					await Server.Core.Assistance.Initializer.Dal.Depot.SaveAsync(connection.DepId, connection.HashId, new List<WebDepot> { depot }, ENSource.Accounting, connection.cancellationToken);
					return ret;
				}
				using DepotWaiter waiter = new DepotWaiter(connection.Context, depot.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebDepot.GenerateSaveRequest(new List<WebDepot> { depot }), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("clsDepot.SendAsync"), ReturnedState.None, "SendDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 217);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 219);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 222);
				ret.AddReturnedValue(ex, "", "SendDepotAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 223);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 300000))
		{
			int line = 0;
			try
			{
				line = 1;
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 238);
				}
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				if (syncInfo == null)
				{
					throw new OperationCanceledException();
				}
				line = 5;
				if (syncInfo.DepotReset)
				{
					line = 6;
					IEnumerable<WebDepot> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.Depot.GetAllAsync(connection.DepId, new DateTime(1900, 1, 1), connection.cancellationToken), connection);
					List<SyncResetItem> ResetItems = new List<SyncResetItem>();
					line = 10;
					if (enumerable != null && enumerable.Count() > 0)
					{
						foreach (WebDepot item in enumerable)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item.Guid,
								Id = item.Id
							});
						}
					}
					line = 20;
					SyncDoneWaiter syncDoneWaiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Depot, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.Depot, ResetItems), Priority.High));
					if (await syncDoneWaiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.Depot, connection.cancellationToken);
					}
				}
				line = 30;
				IEnumerable<WebDepot> lst = await Server.Core.Assistance.Initializer.Dal.Depot.GetAllAsync(connection.DepId, syncInfo.Depot, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 266);
				}
				DateTime syncing = DateTime.Now;
				line = 40;
				lst = await ApplyAccessAsync(lst, connection);
				line = 50;
				if (lst?.Any() ?? false)
				{
					Task.Run(() => connection.SendPacketAsync(WebDepot.GenerateReadResponse(lst, isSync: true), Priority.Low));
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Depot, 300000, connection.cancellationToken);
					line = 55;
					bool flag = connection.Source != ENSource.WebsiteWordPress;
					if (flag)
					{
						flag = !(await waiter.WaitForSyncDonePacket());
					}
					if (flag)
					{
						line = 56;
						ret.AddReturnedValue(ReturnedState.Error, "connection.Source != ENSource.WebsiteWordPress && !await waiter.WaitForSyncDonePacket()", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 283);
						return ret;
					}
					line = 60;
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.Depot, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 288);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 294);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDepot.cs", 295);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
