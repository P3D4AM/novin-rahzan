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
using PacketParser.TransferLayer.Waiters.Accounting.Product;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsProductGroup : IEntity<WebPrdGroup>
{
	private class NestedProductGroup
	{
		internal static readonly ClsProductGroup ProductGroupInstance;

		static NestedProductGroup()
		{
			ProductGroupInstance = new ClsProductGroup();
		}
	}

	public static ClsProductGroup ProductGroupInstance => NestedProductGroup.ProductGroupInstance;

	private ClsProductGroup()
	{
	}

	public async Task<ReturnedSaveFuncInfo> SendGroupAsync(WebPrdGroup group, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using PrdGroupWaiter waiter = new PrdGroupWaiter(connection.Context, group.Guid, connection.cancellationToken);
				connection.SendPacketAsync(WebPrdGroup.GenerateSaveRequest(new List<WebPrdGroup> { group }), Priority.Medium);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("clsProductGroup.SendGroupAsync"), ReturnedState.None, "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 35);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 37);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 40);
				ret.AddReturnedValue(ex, "", "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 41);
			}
			return ret;
		}
	}

	internal async Task PrdGroupInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdGroup> e)
	{
		using (new JaegerMonitor("PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ProductGroup, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebPrdGroup> connectionEventArgs = e;
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
					ret = await Server.Core.Assistance.Initializer.Dal.ProductGroup.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken, 10);
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
					WebErrorLog.Instance.StartLog(ex2, "", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 74);
					ret.Add(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				line = 150;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					line = 151;
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 152;
					if (connection != null && ret != null)
					{
						await connection.SendPacketAsync(WebPrdGroup.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				List<WebPrdGroup> truSavedInRet = e.ConnectionData.Where((WebPrdGroup p) => trueSaved.Contains(p.Guid)).ToList();
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				ConnectionEventArgs<WebPrdGroup> connectionEventArgs2 = e;
				if ((connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebPrdGroup.GenerateSaveRequest(truSavedInRet), Priority.Medium));
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
						IEnumerable<WebPrdGroup> data = truSavedInRet;
						if (Connection.ClientVersion < 10 && Connection.Source != ENSource.WebsiteWordPress)
						{
							data = await ApplyAccessAsync(truSavedInRet, Connection);
						}
						Task.Run(async () => Connection.SendPacketAsync(WebPrdGroup.GenerateReadResponse(data), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, $"Line:{line}", "PrdGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 122);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 300000))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 1;
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 133);
				}
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				line = 2;
				connection?.cancellationToken.ThrowIfCancellationRequested();
				line = 3;
				if (syncInfo == null)
				{
					return ret;
				}
				line = 4;
				if ((connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress) && syncInfo.PrdGroupReset)
				{
					line = 5;
					List<WebPrdGroup> allProductsGroup = await Server.Core.Assistance.Initializer.Dal.ProductGroup.GetAllAsync(connection.DepId, new DateTime(1900, 1, 1), connection.cancellationToken, 10);
					line = 6;
					await ApplyAccessAsync(allProductsGroup, connection);
					line = 7;
					List<SyncResetItem> list = new List<SyncResetItem>();
					line = 8;
					if ((allProductsGroup?.Count() ?? 0) > 0)
					{
						foreach (WebPrdGroup item in allProductsGroup)
						{
							list.Add(new SyncResetItem
							{
								Guid = item.Guid,
								Id = item.Id
							});
						}
					}
					line = 15;
					SyncDoneWaiter syncDoneWaiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.ProductGroup, 300000, connection.cancellationToken);
					line = 16;
					if (connection.Source != ENSource.WebsiteWordPress)
					{
						connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.ProductGroup, list), Priority.High);
					}
					line = 17;
					if (await syncDoneWaiter.WaitForSyncDonePacket())
					{
						line = 18;
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.ProductGroup, connection.cancellationToken);
						line = 19;
					}
				}
				line = 20;
				DateTime syncing = DateTime.Now;
				line = 21;
				List<WebPrdGroup> lst = await Server.Core.Assistance.Initializer.Dal.ProductGroup.GetAllAsync(connection?.DepId ?? 0, syncInfo?.ProductGroup.AddHours(-10.0) ?? DateTime.Now.AddYears(-10), connection?.cancellationToken ?? default(CancellationToken), 10);
				line = 22;
				connection?.cancellationToken.ThrowIfCancellationRequested();
				line = 23;
				List<WebPrdGroup> deletedLst = lst;
				if (connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress)
				{
					lst = await ApplyAccessAsync(lst, connection);
				}
				line = 2500;
				if (lst?.Any() ?? false)
				{
					deletedLst = deletedLst?.Except(lst)?.ToList();
				}
				line = 251;
				if ((connection.ClientVersion < 10 || connection.Source == ENSource.WebsiteWordPress) && (deletedLst?.Any() ?? false))
				{
					deletedLst = deletedLst.Where((WebPrdGroup p) => p.ResetTime > (syncInfo?.ProductGroup ?? DateTime.MinValue)).ToList();
					line = 252;
					if (deletedLst?.Any() ?? false)
					{
						GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, 0, 10000, connection.cancellationToken, EntityPart.ProductGroup, connection.ContextGuid);
						line = 2530;
						if (connection.Source == ENSource.WebsiteWordPress)
						{
							connection.SendPacketAsync(WebPrdGroup.GenerateDeleteRequest(deletedLst), Priority.Low);
						}
						else
						{
							line = 25330;
							IEnumerable<Guid> enumerable = deletedLst?.Select((WebPrdGroup p) => p.Guid);
							line = 50001;
							if (connection?.IsDisposed ?? true)
							{
								return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 208);
							}
							line = 50002;
							if (enumerable == null || !enumerable.Any())
							{
								return ret;
							}
							line = 500030;
							List<Packet> list2 = WebPrdGroup.GenerateDeleteRequest(enumerable);
							line = 500031;
							if (list2 == null || !list2.Any())
							{
								throw new Exception("if (!(packs?.Any() ?? false))");
							}
							line = 500032;
							connection.SendPacketAsync(list2, Priority.Low, 0, connection.cancellationToken);
							line = 2534;
						}
						line = 254;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 221);
						}
						if (!(await getDeleteResponsePacket.WaitForDeleteDonePacket()) && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
						{
							ret.AddReturnedValue(ReturnedState.Error, "ResetTimeFlag", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 225);
							return ret;
						}
					}
				}
				line = 28;
				if (lst?.Any() ?? false)
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.ProductGroup, (connection.Source == ENSource.WebsiteWordPress) ? 1800000 : 300000, connection.cancellationToken);
					line = 29;
					connection.SendPacketAsync(WebPrdGroup.GenerateReadResponse(lst, isSync: true), Priority.Low);
					line = 30;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddError("waiter timeout WebPrdGroup.GenerateReadResponse", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 242);
						return ret;
					}
					line = 31;
					ret.throwExceptionIfError();
				}
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.ProductGroup, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 249);
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 251);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 254);
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 255);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	public async Task<List<WebPrdGroup>> ApplyAccessAsync(IEnumerable<WebPrdGroup> lst, IUserAcc UserAccInfo, ENSource source, int depid, bool IsAccountingFamily = true, CancellationToken token = default(CancellationToken))
	{
		try
		{
			if (!IsAccountingFamily || (lst?.Count() ?? 0) <= 0)
			{
				return null;
			}
			if (!IsAccountingFamily)
			{
				return null;
			}
			if (source != ENSource.Accounting)
			{
				lst?.Count();
				lst = lst?.Where((WebPrdGroup p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
			if (!(UserAccInfo?.UserAccess?.Product?.Product_DastrasiBeKalahayeKhas).GetValueOrDefault(true))
			{
				lst = lst?.Where((WebPrdGroup p) => !p.Special);
			}
			if ((UserAccInfo?.UserAccess?.PrdGroupListGuid?.Any()).GetValueOrDefault())
			{
				lst = lst?.Where((WebPrdGroup e) => UserAccInfo.UserAccess.PrdGroupListGuid.Any((Guid e2) => e2.Equals(e.Guid)));
			}
			else
			{
				IUserAcc userAcc = UserAccInfo;
				if (userAcc != null && userAcc.UserAccess?.PrdGroupList?.Count > 0)
				{
					lst = lst?.Where((WebPrdGroup e) => UserAccInfo.UserAccess.PrdGroupList.Any((short e2) => e2.Equals(e.Id)));
				}
			}
			return lst?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 290);
			throw;
		}
	}

	public async Task<List<WebPrdGroup>> ApplyAccessAsync(IEnumerable<WebPrdGroup> lst, IClsConnectionData connection)
	{
		return await ApplyAccessAsync(lst, await connection.GetUserAccInfoAsync(), connection.Source, connection.DepId, connection.IsAccountingFamily);
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, short maxParallelTasks = 5)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"source({connection.Source})!=Accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 305);
				}
				line = 1;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.ProductGroup, connection.cancellationToken);
				line = 2;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 309);
				}
				line = 3;
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				line = 4;
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list ?? new List<Guid>())
				{
					line = 5;
					ret.AddInformation($"sending prdGroup({item}) to accounting connection", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 317);
					line = 6;
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 319);
					}
					line = 7;
					WebPrdGroup webPrdGroup = await Server.Core.Assistance.Initializer.Dal.ProductGroup.LoadDataAsync(connection.DepId, item, connection.cancellationToken, 10);
					line = 8;
					if (webPrdGroup != null)
					{
						line = 9;
						tasks.Add(SendGroupAsync(webPrdGroup, connection));
						line = 10;
						if (tasks.Count > maxParallelTasks)
						{
							line = 11;
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
							line = 12;
							tasks.Remove(task);
							line = 13;
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 334);
						}
						line = 14;
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 338);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 342);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 347);
				WebErrorLog.Instance.StartLog(ex, "#ServerHubStandard\r\n" + $"#Line_{line}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsProductGroup.cs", 348);
			}
			return ret;
		}
	}
}
