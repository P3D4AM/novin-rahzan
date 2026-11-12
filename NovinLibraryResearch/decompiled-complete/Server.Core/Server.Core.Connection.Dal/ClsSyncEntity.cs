using System;
using System.Collections.Concurrent;
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
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsSyncEntity : IEntity<WebEntitySync>
{
	private class NestedSyncEntity
	{
		internal static readonly ClsSyncEntity Instance;

		static NestedSyncEntity()
		{
			Instance = new ClsSyncEntity();
		}
	}

	private static ConcurrentDictionary<short, bool> _dicIsNewDescExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsSyncEntity Instance => NestedSyncEntity.Instance;

	private ClsSyncEntity()
	{
	}

	public Task<List<WebEntitySync>> ApplyAccessAsync(IEnumerable<WebEntitySync> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebEntitySync>>(null);
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebEntitySync p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task SyncInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebEntitySync> e)
	{
		if (e?.ConnectionData == null)
		{
			return;
		}
		ConnectionEventArgs<WebEntitySync> connectionEventArgs = e;
		if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
		{
			return;
		}
		IEnumerable<SaveResponse> ret2 = new List<SaveResponse>();
		using (new JaegerMonitor("SyncInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.EntitySync, 1, isReceivedPacket: true);
				if (!(e.ConnectionData?.Any() ?? false))
				{
					throw new Exception("Connection contains no element");
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				e?.Connection?.cancellationToken.ThrowIfCancellationRequested();
				if ((e?.Connection?.DepId).GetValueOrDefault() <= 0)
				{
					throw new OperationCanceledException();
				}
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				ret2 = await Server.Core.Assistance.Initializer.Dal.SyncEntity.Save(connection.DepId, connection.HashId, e.ConnectionData, connection.Source, connection.cancellationToken);
				if (ret2 == null)
				{
					throw new NullReferenceException("ret");
				}
				foreach (SaveResponse item in ret2)
				{
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					item.PartitionId = e.PartitionId;
				}
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection2 != null)
					{
						await connection2.SendPacketAsync(WebEntitySync.GenerateSaveResponse(ret2), Priority.High, 0, e.Connection.cancellationToken);
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					ClsConnectionData cn2 = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (!(cn2?.IsDisposed ?? true))
					{
						Task.Run(() => cn2.SendPacketAsync(WebEntitySync.GenerateSaveRequest(e.ConnectionData, 0, "ServerHub.OnSaveRequest"), Priority.High, 0, e.Connection.cancellationToken));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData cn in enumerable)
				{
					if ((cn?.IsDisposed ?? true) || cn.ClientVersion < 13)
					{
						continue;
					}
					List<WebEntitySync> SendLst = await Instance.ApplyAccessAsync(e.ConnectionData, cn);
					if (SendLst?.Any() ?? false)
					{
						Task.Run(() => cn.SendPacketAsync(WebEntitySync.GenerateReadResponse(SendLst, 0), Priority.Low, 0, cn.cancellationToken));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex)
			{
				ret2 = ret2.Append(new SaveResponse
				{
					SaveResultStatus = false
				});
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ClsConnectionData connection3 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				if (connection3 != null)
				{
					await connection3.SendPacketAsync(WebIntroductionMethod.GenerateSaveResponse(ret2), Priority.High);
				}
				WebErrorLog.Instance.StartLog(ex, "", "SyncInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 93);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				return ret;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 142);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 145);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 146);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionsAsync(IEnumerable<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			new CancellationTokenSource();
			if (!(partitions?.Any() ?? false))
			{
				return ret;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (short partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
					tasks.Remove(task);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 166);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 169);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 171);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 174);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 175);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 186);
				}
				IEnumerable<WebEntitySync> descs = await (Server.Core.Assistance.Initializer.Dal?.SyncEntity?.GetAllAsync(connection, partition));
				if (descs == null)
				{
					goto IL_0195;
				}
				IEnumerable<WebEntitySync> enumerable = descs;
				if (enumerable != null && enumerable.Count() <= 0)
				{
					goto IL_0195;
				}
				descs = await ApplyAccessAsync(descs, connection);
				if (descs?.Any() ?? false)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 193);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.EntitySync, 20000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebEntitySync.GenerateReadResponse(descs, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						return new ReturnedSaveFuncInfo(ReturnedState.Error, "TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 197);
					}
					ret.throwExceptionIfError();
					await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.EntitySync, syncing, connection);
				}
				goto end_IL_0080;
				IL_0195:
				return ret;
				end_IL_0080:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 202);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 205);
				ret.AddReturnedValue(ex, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 206);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliverd(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				return res;
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 233);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 236);
				res.AddReturnedValue(ex2, "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsSyncEntity.cs", 237);
			}
			return res;
		}
	}
}
