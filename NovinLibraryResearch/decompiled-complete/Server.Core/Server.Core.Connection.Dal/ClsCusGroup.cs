using System;
using System.Collections.Concurrent;
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

public class ClsCusGroup : IEntity<WebCusGroup>
{
	private class NestedClsCusGroup
	{
		internal static readonly ClsCusGroup CusGroupInstance;

		static NestedClsCusGroup()
		{
			CusGroupInstance = new ClsCusGroup();
		}
	}

	private static ConcurrentDictionary<short, DateTime> dic_isModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	public static ClsCusGroup CusGroupInstance => NestedClsCusGroup.CusGroupInstance;

	private ClsCusGroup()
	{
	}

	public async Task<List<WebCusGroup>> ApplyAccessAsync(IEnumerable<WebCusGroup> lst, IClsConnectionData connection)
	{
		_ = 1;
		try
		{
			if (!connection.IsAccountingFamily)
			{
				return null;
			}
			if (connection.Source != ENSource.Accounting)
			{
				lst?.Count();
				lst = lst?.Where((WebCusGroup p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
			WebDepartment department = await connection.GetDepartmentAsync();
			if (!connection.IsAccountingFamily)
			{
				return null;
			}
			if (lst != null)
			{
				IEnumerable<WebCusGroup> enumerable = lst;
				if (enumerable == null || enumerable.Count() > 0)
				{
					WebUserAcc userInfo = await connection.GetUserAccInfoAsync();
					WebUserAcc webUserAcc = userInfo;
					if (webUserAcc != null && webUserAcc.UserAccess?.Customers?.Customer_DastrasiBeAshkhaseKhas == false)
					{
						lst = lst.Where((WebCusGroup e) => !e.Special)?.ToList();
					}
					if (!department.IsAccountingVerion6)
					{
						WebUserAcc webUserAcc2 = userInfo;
						if (webUserAcc2 != null && webUserAcc2.UserAccess?.TafsilGroupList?.Count > 0)
						{
							lst = lst?.Where((WebCusGroup e) => userInfo.UserAccess.TafsilGroupList.Any((short e2) => e2.Equals(e.TafsilLevelId))).ToList();
						}
					}
					return lst?.ToList();
				}
			}
			return lst?.ToList();
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 55);
			return null;
		}
	}

	internal async Task CusGroupInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCusGroup> e)
	{
		using (new JaegerMonitor("CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.CustomerGroup, 1, isReceivedPacket: true);
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.CusGroup.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
					WebErrorLog.Instance.StartLog(ex2, "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 93);
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.SendPacketAsync(WebCusGroup.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				List<WebCusGroup> truSavedInRet = e.ConnectionData.Where((WebCusGroup p) => trueSaved.Contains(p.Guid)).ToList();
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				if ((e == null || e.Connection.Source != ENSource.Accounting) && truSavedInRet.Count() > 0)
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion > 8)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebCusGroup.GenerateSaveRequest(truSavedInRet, 0), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion >= 10);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData Connection in enumerable)
				{
					IEnumerable<WebCusGroup> dataForSend = truSavedInRet;
					if (Connection.ClientVersion < 10 && Connection.Source != ENSource.WebsiteWordPress)
					{
						dataForSend = await ApplyAccessAsync(dataForSend, Connection);
					}
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(async () => Connection.SendPacketAsync(WebCusGroup.GenerateReadResponse(dataForSend), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "CusGroupInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 140);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress)
		{
			return ret;
		}
		int line = 0;
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 300000))
		{
			_ = 9;
			try
			{
				line = 1;
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 153);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				line = 2;
				if (syncInfo == null)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection?.SyncInfo == null", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 159);
					line = 3;
					return ret;
				}
				line = 4;
				if (connection.ClientVersion < 10 && syncInfo.CusGroupReset)
				{
					line = 5;
					IEnumerable<WebCusGroup> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.CusGroup.GetAllAsync(connection.DepId, new DateTime(2000, 1, 1), connection.cancellationToken), connection);
					List<SyncResetItem> list = new List<SyncResetItem>();
					line = 6;
					if (enumerable != null && enumerable.Count() > 0)
					{
						foreach (WebCusGroup item in enumerable)
						{
							list.Add(new SyncResetItem
							{
								Guid = item.Guid,
								Id = item.Id
							});
						}
					}
					line = 7;
					SyncDoneWaiter syncDoneWaiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.CustomerGroup, 300000, connection.cancellationToken);
					line = 8;
					connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.CustomerGroup, list), Priority.High);
					line = 9;
					if (await syncDoneWaiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.CustomerGroup, connection.cancellationToken);
					}
					line = 10;
				}
				DateTime syncing = DateTime.Now;
				line = 110;
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection is null or disposed!", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 191);
				}
				line = 111;
				if (syncInfo == null)
				{
					throw new NullReferenceException("syncInfo is null");
				}
				IEnumerable<WebCusGroup> lst = await Server.Core.Assistance.Initializer.Dal.CusGroup.GetAllAsync(connection.DepId, syncInfo.CustomerGroup, connection.cancellationToken);
				line = 12;
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection == null || connection.IsDisposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 200);
					return ret;
				}
				line = 13;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 204);
				}
				line = 14;
				IEnumerable<WebCusGroup> deletedLst2 = lst;
				if (connection.ClientVersion < 10 && connection.Source != ENSource.WebsiteWordPress)
				{
					lst = await ApplyAccessAsync(lst, connection);
				}
				line = 150;
				if (lst != null)
				{
					line = 151;
					deletedLst2 = deletedLst2.Except(lst);
					line = 152;
				}
				line = 153;
				if (connection.ClientVersion < 10 && (deletedLst2?.Any() ?? false))
				{
					if (syncInfo == null)
					{
						throw new NullReferenceException("connection?.SyncInfo is null!!");
					}
					line = 154;
					deletedLst2 = deletedLst2?.Where((WebCusGroup p) => p != null && p.ResetTime > syncInfo.CustomerGroup);
					line = 15500;
					bool num = deletedLst2?.Any() ?? false;
					line = 1551;
					if (num)
					{
						line = 156;
						if (connection?.IsDisposed ?? true)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 230);
						}
						GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, 0, 10000, connection.cancellationToken, EntityPart.CustomerGroup, connection.ContextGuid);
						line = 170;
						IEnumerable<Guid> items = deletedLst2?.Select((WebCusGroup p) => p.Guid);
						line = 171;
						if (items?.Any() ?? false)
						{
							line = 172;
							if (connection?.IsDisposed ?? false)
							{
								line = 173;
								Task.Run(() => connection.SendPacketAsync(WebCusGroup.GenerateDeleteRequest(items), Priority.Low));
							}
						}
						line = 175;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 248);
						}
						line = 18;
						if (!(await getDeleteResponsePacket.WaitForDeleteDonePacket()) && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
						{
							ret.AddReturnedValue(ReturnedState.Error, "Server.Core.Assistance.Initializer.Dal.ResetTimeFlag", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 253);
							return ret;
						}
						line = 19;
					}
					line = 5000;
				}
				line = 5001;
				line = 24;
				if (lst?.Any() ?? false)
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.CustomerGroup, 300000, connection.cancellationToken);
					line = 23;
					Task.Run(() => connection.SendPacketAsync(WebCusGroup.GenerateReadResponse(lst, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "!await waiter.WaitForSyncDonePacket()//ClsCusGroup.Sync2Client", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 283);
						return ret;
					}
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.CustomerGroup, syncing, connection), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 287);
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 292);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 293);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"CusGroupSyncResult:{!ret.HasError}");
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendGroupAsync(WebCusGroup group, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using CusGroupWaiter waiter = new CusGroupWaiter(connection.Context, group.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebCusGroup.GenerateSaveRequest(new List<WebCusGroup> { group }, 0), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("clsCusGroup.SendGroupAsync"), ReturnedState.None, "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 311);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 313);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 316);
				ret.AddReturnedValue(ex, "", "SendGroupAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 317);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs"))
		{
			int line = 0;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Connection({connection.Source}) != Accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 329);
				}
				line = 1;
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.CustomerGroup, connection.cancellationToken);
				line = 2;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 333);
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
						ret.AddInformation($"Sending CustomerGroup({item}) to accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 341);
						line = 6;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 343);
						}
						line = 7;
						WebCusGroup webCusGroup = (WebCusGroup)(await Server.Core.Assistance.Initializer.Dal.CusGroup.GetAsync(connection.DepId, item));
						line = 8;
						if (webCusGroup != null)
						{
							line = 9;
							tasks.Add(SendGroupAsync(webCusGroup, connection));
							if (tasks.Count >= maxParalelTasks)
							{
								line = 10;
								Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
								line = 11;
								tasks.Remove(task);
								line = 12;
								returnedSaveFuncInfo = ret;
								returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 357);
							}
							line = 13;
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 361);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 365);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 370);
				WebErrorLog.Instance.StartLog(ex, "#ServerHub\r\n" + $"#Line_{line}", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCusGroup.cs", 371);
			}
			return ret;
		}
	}
}
