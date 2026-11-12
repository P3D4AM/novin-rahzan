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

public class ClsIntroductionMethod : IEntity<WebIntroductionMethod>
{
	private class NestedIntroductionMethod
	{
		internal static readonly ClsIntroductionMethod IntroductionMethodInstance;

		static NestedIntroductionMethod()
		{
			IntroductionMethodInstance = new ClsIntroductionMethod();
		}
	}

	private static ConcurrentDictionary<short, bool> _dicIsNewDescExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsIntroductionMethod IntroductionMethodInstance => NestedIntroductionMethod.IntroductionMethodInstance;

	private ClsIntroductionMethod()
	{
	}

	public Task<List<WebIntroductionMethod>> ApplyAccessAsync(IEnumerable<WebIntroductionMethod> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebIntroductionMethod>>(null);
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebIntroductionMethod p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task DescInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebIntroductionMethod> e)
	{
		if (e?.ConnectionData == null)
		{
			return;
		}
		ConnectionEventArgs<WebIntroductionMethod> connectionEventArgs = e;
		if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
		{
			return;
		}
		IEnumerable<SaveResponse> ret2 = new List<SaveResponse>();
		using (new JaegerMonitor("DescInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.IntroductionMethod, 1, isReceivedPacket: true);
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
				ret2 = await Server.Core.Assistance.Initializer.Dal.IntroductionMethod.Save(connection.DepId, connection.HashId, e.ConnectionData, connection.Source, connection.cancellationToken);
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
						await connection2.SendPacketAsync(WebIntroductionMethod.GenerateSaveResponse(ret2), Priority.High, 0, e.Connection.cancellationToken);
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
						Task.Run(() => cn2.SendPacketAsync(WebIntroductionMethod.GenerateSaveRequest(e.ConnectionData), Priority.High, 0, e.Connection.cancellationToken));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion > 12);
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
					List<WebIntroductionMethod> SendLst = await IntroductionMethodInstance.ApplyAccessAsync(e.ConnectionData, cn);
					if (SendLst?.Any() ?? false)
					{
						Task.Run(() => cn.SendPacketAsync(WebIntroductionMethod.GenerateReadResponse(SendLst, 0), Priority.Low, 0, cn.cancellationToken));
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
				WebErrorLog.Instance.StartLog(ex, "", "DescInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 97);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.ClientVersion <= 12)
				{
					return ret;
				}
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 108);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				if (connection == null || connection.IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 111);
				}
				if (syncInformation == null)
				{
					throw new NullReferenceException("syncInfo");
				}
				if (syncInformation.IntroductionMethodReset)
				{
					IEnumerable<WebIntroductionMethod> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.IntroductionMethod.GetAllAsync(connection, -1), connection);
					List<SyncResetItem> resetItems = new List<SyncResetItem>();
					if (enumerable != null && enumerable.Any())
					{
						foreach (WebIntroductionMethod item in enumerable)
						{
							resetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.IntroductionMethod, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.IntroductionMethod, resetItems), Priority.High));
					if (await waiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.IntroductionMethod, connection.cancellationToken);
					}
				}
				List<short> totalPartitions = new List<short>();
				IEnumerable<short> enumerable2 = await Server.Core.Assistance.Initializer.Dal.IntroductionMethod.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (enumerable2 != null && enumerable2.Any())
				{
					totalPartitions.AddRange(enumerable2);
					while (enumerable2 != null && enumerable2.Any())
					{
						if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 137);
							return ret;
						}
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SyncPartitionsAsync(enumerable2, connection, ConnectionUniqueId, maxParalel), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 140);
						enumerable2 = (await Server.Core.Assistance.Initializer.Dal.IntroductionMethod.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => !totalPartitions.Contains(x));
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 146);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 149);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 150);
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
					returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 171);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 174);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 176);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 179);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 180);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 192);
				}
				IEnumerable<WebIntroductionMethod> descs = await Server.Core.Assistance.Initializer.Dal.IntroductionMethod.GetAllAsync(connection, partition);
				if (descs == null)
				{
					goto IL_0187;
				}
				IEnumerable<WebIntroductionMethod> enumerable = descs;
				if (enumerable != null && enumerable.Count() <= 0)
				{
					goto IL_0187;
				}
				descs = await ApplyAccessAsync(descs, connection);
				if (descs?.Any() ?? false)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 199);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.IntroductionMethod, 20000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebIntroductionMethod.GenerateReadResponse(descs, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						return new ReturnedSaveFuncInfo(ReturnedState.Error, "TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 203);
					}
					ret.throwExceptionIfError();
					await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.IntroductionMethod, syncing, connection);
				}
				goto end_IL_0080;
				IL_0187:
				return ret;
				end_IL_0080:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 208);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 211);
				ret.AddReturnedValue(ex, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 212);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliverd(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs"))
		{
			ReturnedSaveFuncInfo res = new ReturnedSaveFuncInfo();
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.IntroductionMethod, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return res.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 224);
				}
				if (list == null || (list != null && list.Count <= 0))
				{
					_dicIsNewDescExistsFromClient.TryRemove(connection.DepId, out var _);
					_dicIsNewDescExistsFromClient.TryAdd(connection.DepId, value: false);
					return res;
				}
				foreach (Guid item in list)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return res.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 233);
					}
					WebIntroductionMethod webIntroductionMethod = await Server.Core.Assistance.Initializer.Dal.IntroductionMethod.LoadDataAsync(item);
					if (webIntroductionMethod != null)
					{
						await connection.SendPacketAsync(WebIntroductionMethod.GenerateSaveRequest(new List<WebIntroductionMethod> { webIntroductionMethod }), Priority.Medium);
					}
				}
				return res;
			}
			catch (OperationCanceledException)
			{
				res.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 240);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 243);
				res.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsIntroductionMethod.cs", 244);
			}
			return res;
		}
	}
}
