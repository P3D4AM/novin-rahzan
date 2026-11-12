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

public class ClsDesc : IEntity<WebDesc>
{
	private class NestedDesc
	{
		internal static readonly ClsDesc DescInstance;

		static NestedDesc()
		{
			DescInstance = new ClsDesc();
		}
	}

	private static ConcurrentDictionary<short, bool> _dicIsNewDescExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsDesc DescInstance => NestedDesc.DescInstance;

	private ClsDesc()
	{
	}

	public Task<List<WebDesc>> ApplyAccessAsync(IEnumerable<WebDesc> lst, IClsConnectionData connection)
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
				lst = lst?.Where((WebDesc p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 34);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task DescInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebDesc> e)
	{
		if (e?.ConnectionData == null)
		{
			return;
		}
		ConnectionEventArgs<WebDesc> connectionEventArgs = e;
		if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
		{
			return;
		}
		IEnumerable<SaveResponse> ret2 = new List<SaveResponse>();
		int line = 1;
		using (new JaegerMonitor("DescInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs"))
		{
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Desc, 1, isReceivedPacket: true);
				if (!(e.ConnectionData?.Any() ?? false))
				{
					throw new Exception("Connection contains no element");
				}
				ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				line = 2;
				e?.Connection?.cancellationToken.ThrowIfCancellationRequested();
				line = 3;
				if ((e?.Connection?.DepId).GetValueOrDefault() <= 0)
				{
					throw new OperationCanceledException();
				}
				if (e.Connection.cancellationToken.IsCancellationRequested)
				{
					return;
				}
				line = 5;
				ret2 = await Server.Core.Assistance.Initializer.Dal.Desc.SaveAsync(connection.DepId, connection.HashId, e.ConnectionData, connection.Source, connection.cancellationToken);
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
				line = 6;
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
					line = 7;
					ClsConnectionData connection2 = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 8;
					if (connection2 != null)
					{
						await connection2.SendPacketAsync(WebDesc.GenerateSaveResponse(ret2), Priority.High, 0, e.Connection.cancellationToken);
					}
					line = 9;
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					if (e.Connection.cancellationToken.IsCancellationRequested)
					{
						return;
					}
					line = 10;
					ClsConnectionData cn2 = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					line = 11;
					if (!(cn2?.IsDisposed ?? true))
					{
						Task.Run(() => cn2.SendPacketAsync(WebDesc.GenerateSaveRequest(e.ConnectionData), Priority.High, 0, e.Connection.cancellationToken));
					}
					line = 12;
				}
				line = 15;
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
					if (cn?.IsDisposed ?? true)
					{
						continue;
					}
					line = 17;
					List<WebDesc> SendLst = await DescInstance.ApplyAccessAsync(e.ConnectionData, cn);
					if (SendLst?.Any() ?? false)
					{
						Task.Run(() => cn.SendPacketAsync(WebDesc.GenerateReadResponse(SendLst, 0), Priority.Low, 0, cn.cancellationToken));
					}
					line = 18;
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
					await connection3.SendPacketAsync(WebCus.GenerateSaveResponse(ret2), Priority.High);
				}
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "DescInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 117);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1000;
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 130);
				}
				line = 1001;
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				line = 100200;
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				if (connection == null || connection.IsDisposed)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 135);
				}
				if (syncInformation == null)
				{
					throw new NullReferenceException("syncInfo");
				}
				line = 10021;
				if (syncInformation.DescReset)
				{
					line = 5;
					IEnumerable<WebDesc> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.Desc.GetAllAsync(connection, -1), connection);
					List<SyncResetItem> resetItems = new List<SyncResetItem>();
					line = 10;
					if (enumerable != null && enumerable.Any())
					{
						foreach (WebDesc item in enumerable)
						{
							resetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					line = 20;
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Desc, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.Desc, resetItems), Priority.High));
					line = 25;
					if (await waiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.Desc, connection.cancellationToken);
					}
				}
				line = 30;
				List<short> totalPartitions = new List<short>();
				IEnumerable<short> enumerable2 = await Server.Core.Assistance.Initializer.Dal.Desc.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				line = 35;
				if (enumerable2 != null && enumerable2.Any())
				{
					line = 36;
					totalPartitions.AddRange(enumerable2);
					while (enumerable2 != null && enumerable2.Any())
					{
						line = 40;
						if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							line = 41;
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 172);
							return ret;
						}
						line = 45;
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SyncPartitionsAsync(enumerable2, connection, ConnectionUniqueId, maxParalel), ReturnedState.None, "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 176);
						enumerable2 = (await Server.Core.Assistance.Initializer.Dal.Desc.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => !totalPartitions.Contains(x));
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 182);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 185);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 186);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
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
					returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 207);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 210);
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 212);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 215);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 216);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				DateTime syncing = DateTime.Now;
				line = 5;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 230);
				}
				IEnumerable<WebDesc> descs = await Server.Core.Assistance.Initializer.Dal.Desc.GetAllAsync(connection, partition);
				if (descs == null)
				{
					goto IL_0195;
				}
				IEnumerable<WebDesc> enumerable = descs;
				if (enumerable != null && enumerable.Count() <= 0)
				{
					goto IL_0195;
				}
				descs = await ApplyAccessAsync(descs, connection);
				line = 10;
				if (descs?.Any() ?? false)
				{
					line = 15;
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 245);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Desc, 20000, connection.cancellationToken);
					line = 18;
					Task.Run(() => connection.SendPacketAsync(WebDesc.GenerateReadResponse(descs, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						return new ReturnedSaveFuncInfo(ReturnedState.Error, "TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 250);
					}
					line = 20;
					ret.throwExceptionIfError();
					await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Desc, syncing, connection);
				}
				goto end_IL_0087;
				IL_0195:
				return ret;
				end_IL_0087:;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 256);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 259);
				ret.AddReturnedValue(ex, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 260);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliverd(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Desc, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 316);
				}
				if (list == null || (list != null && list.Count <= 0))
				{
					_dicIsNewDescExistsFromClient.TryRemove(connection.DepId, out var _);
					_dicIsNewDescExistsFromClient.TryAdd(connection.DepId, value: false);
					return ret;
				}
				foreach (Guid item in list)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 325);
					}
					WebDesc webDesc = await Server.Core.Assistance.Initializer.Dal.Desc.LoadDataAsync(item);
					if (webDesc != null)
					{
						await connection.SendPacketAsync(WebDesc.GenerateSaveRequest(new List<WebDesc> { webDesc }), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 331);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 334);
				ret.AddReturnedValue(ex, "", "SendNotDeliverd", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsDesc.cs", 335);
			}
			return ret;
		}
	}
}
