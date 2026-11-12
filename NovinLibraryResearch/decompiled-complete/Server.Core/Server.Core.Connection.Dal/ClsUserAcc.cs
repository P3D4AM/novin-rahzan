using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Access;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsUserAcc : IEntity<WebUserAcc>
{
	private class NestedUserAcc
	{
		internal static readonly ClsUserAcc UserAccInstance;

		static NestedUserAcc()
		{
			UserAccInstance = new ClsUserAcc();
		}
	}

	public static ClsUserAcc UserAccInstance => NestedUserAcc.UserAccInstance;

	private ClsUserAcc()
	{
	}

	public async Task<List<WebUserAcc>> ApplyAccessAsync(IEnumerable<WebUserAcc> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebUserAcc p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task UserAccInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebUserAcc> e)
	{
		using (new JaegerMonitor("UserAccInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.UserAcc, 1, isReceivedPacket: true);
				if (e == null)
				{
					return;
				}
				ConnectionEventArgs<WebUserAcc> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				List<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection == null)
					{
						return;
					}
					List<UserAccessChanges> result2 = await Server.Core.Assistance.Initializer.Dal.UserAcc.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, connection.cancellationToken);
					if (e.Connection.Source == ENSource.Accounting)
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.ResetSyncsAsync(e.Connection.DepId, result2, connection.cancellationToken);
						e.ConnectionData.Select((WebUserAcc x) => x.Id);
						IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData c) => c.HashId != e.Connection.HashId);
						if (enumerable != null && enumerable.Any())
						{
							foreach (ClsConnectionData item in enumerable)
							{
								if (item.ClientVersion < 10)
								{
									item?.Dispose("User Access Changed!");
								}
							}
						}
					}
					else
					{
						List<WebUserAcc> connectionData = e.ConnectionData;
						if (connectionData != null && connectionData.Count > 0)
						{
							ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebUserAcc.GenerateSaveRequest(e.ConnectionData), Priority.Medium);
						}
					}
					IEnumerable<SaveResponse> collection = e.ConnectionData.Select((WebUserAcc p) => new SaveResponse
					{
						Guid = p.Guid,
						SaveResultStatus = true,
						Date = DateTime.Now
					});
					ret.AddRange(collection);
					if (ret?.All((SaveResponse p) => p.SaveResultStatus) ?? false)
					{
						List<Packet> packs = WebUserAcc.GenerateReadResponse(e.ConnectionData, 0);
						foreach (ClsConnectionData item2 in ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.Guid != e.ConnectionUniqueId && p.IsAccountingFamily && p.Source != ENSource.Accounting) ?? new List<ClsConnectionData>())
						{
							item2.SendPacketAsync(packs, Priority.High);
						}
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex3)
				{
					Exception ex4 = ex3;
					Exception ex = ex4;
					IEnumerable<SaveResponse> collection2 = e?.ConnectionData?.Select((WebUserAcc p) => new SaveResponse
					{
						Guid = p.Guid,
						SaveResultStatus = false,
						Message = ex.Message,
						Shortmessage = ex.Message,
						Date = DateTime.Now
					});
					ret.AddRange(collection2);
					WebErrorLog.Instance.StartLog(ex, "", "UserAccInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 115);
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebUserAcc.GenerateSaveResponse(ret), Priority.High);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex6)
			{
				WebErrorLog.Instance.StartLog(ex6, "", "UserAccInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 122);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDelivered(ClsConnectionData connection, string ConnectionUniqueId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs"))
		{
			try
			{
				List<WebUserAcc> lst = await Server.Core.Assistance.Initializer.Dal.UserAcc.GetallFromClients_ListForServerAsync(connection.DepId, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 132);
				}
				if (!(lst?.Any() ?? false))
				{
					return ret.AddInformation("کاربری برای سینک با سرور وجود ندارد.", "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 133);
				}
				Task.Run(() => connection.SendPacketAsync(WebUserAcc.GenerateSaveRequest(lst), Priority.Medium));
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 138);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 143);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDelivered", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 144);
			}
			return ret;
		}
	}

	public static async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, short maxParalelTasks = 1, int waiterDelayTimeSeconds = 60000)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs"))
		{
			int line = 0;
			try
			{
				if (connection.ClientVersion < 10)
				{
					return ret;
				}
				line = 1;
				if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 158);
				}
				line = 2;
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				if ((connection?.IsDisposed ?? true) || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 161);
				}
				if (syncInfo == null)
				{
					throw new NullReferenceException("syncInfo is null!! چرا باید سینک اینفو نال باشه !؟");
				}
				line = 3;
				syncInfo.User_Syncing = DateTime.Now;
				line = 4;
				List<WebUserAcc> list = await Server.Core.Assistance.Initializer.Dal.UserAcc.GetAllAsync(connection.DepId, syncInfo?.User ?? DateTime.Now.AddYears(-10), connection.cancellationToken);
				line = 5;
				line = 8;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 176);
				}
				line = 9;
				if (list != null && list.Any())
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.User, 300000, connection.cancellationToken);
					line = 10;
					connection.SendPacketAsync(WebUserAcc.GenerateReadResponse(list, 0, isSync: true), Priority.Low);
					line = 11;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						line = 12;
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket(clsuserAcc) timeOut", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 187);
						return ret;
					}
					line = 13;
					if (connection?.IsDisposed ?? true)
					{
						return ret;
					}
					line = 14;
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.User, syncInfo.User_Syncing, connection), ReturnedState.None, "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 194);
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 200);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 204);
				ret.AddReturnedValue(ex, $"#Line_{line}", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsUserAcc.cs", 205);
			}
			return ret;
		}
	}
}
