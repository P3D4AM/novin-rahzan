using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class clsAghsatMGroup : IEntity<WebAghsatMGroup>
{
	private class NestedAghsatMGroup
	{
		internal static readonly clsAghsatMGroup Instance;

		static NestedAghsatMGroup()
		{
			Instance = new clsAghsatMGroup();
		}
	}

	public static clsAghsatMGroup Instance => NestedAghsatMGroup.Instance;

	public Task<List<WebAghsatMGroup>> ApplyAccessAsync(IEnumerable<WebAghsatMGroup> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebAghsatMGroup p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task<ReturnedSaveFuncInfo> AghsatMGroupInstance_OnSaveRequestAsync(object sender, ConnectionEventArgs<WebAghsatMGroup> e)
	{
		using (new JaegerMonitor("AghsatMGroupInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					throw new NullReferenceException("e.connectionData has no data!");
				}
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.AghsatMGroup, 1, isReceivedPacket: true);
				if (e.Connection == null || e.Connection.IsDisposed || e.Connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "AghsatMGroupInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 44);
				}
				ConnectionEventArgs<WebAghsatMGroup> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return ret;
				}
				IEnumerable<SaveResponse> lctSaveResponses = new List<SaveResponse>();
				bool result = true;
				try
				{
					lctSaveResponses = await Server.Core.Assistance.Initializer.Dal.AghsatMGroup.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in lctSaveResponses)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					ret.AddErrorTokenIsCancelled("", "", "AghsatMGroupInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 54);
					return ret;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "AghsatMGroupInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 57);
					lctSaveResponses = lctSaveResponses.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.SendPacketAsync(WebAghsatMGroup.GenerateSaveResponse(lctSaveResponses), Priority.High);
					}
				}
				ConnectionEventArgs<WebAghsatMGroup> connectionEventArgs2 = e;
				if (connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting)
				{
					ConnectionEventArgs<WebAghsatMGroup> connectionEventArgs3 = e;
					if (connectionEventArgs3 != null && connectionEventArgs3.ConnectionData?.Count > 0)
					{
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebAghsatMGroup.GenerateSaveRequest(e.ConnectionData), Priority.Medium);
					}
				}
				List<WebAghsatMGroup> connectionData = e.ConnectionData;
				if (connectionData != null && connectionData.Count > 0)
				{
					List<ClsConnectionData> list = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting)).ToList();
					if (list != null)
					{
						foreach (ClsConnectionData Connection in list)
						{
							if (Connection == null || Connection.IsDisposed)
							{
								continue;
							}
							List<WebAghsatMGroup> list2 = await ApplyAccessAsync(e.ConnectionData, Connection);
							if (list2 != null && list2.Any())
							{
								List<Packet> packs = WebAghsatMGroup.GenerateReadResponse(list2, 0);
								if (Connection?.IsDisposed ?? true)
								{
									return ret;
								}
								Connection.SendPacketAsync(packs, Priority.Medium);
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				ret.AddReturnedValue(ex3, "", "AghsatMGroupInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 92);
				WebErrorLog.Instance.StartLog(ex3, "", "AghsatMGroupInstance_OnSaveRequestAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 93);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebAghsatMGroup.GenerateSaveResponse(responses), Priority.High);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				connection?.cancellationToken.ThrowIfCancellationRequested();
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection isDisposed or SyncInfo is null!!", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 107);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				if (connection.ClientVersion < 21)
				{
					return ret;
				}
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.AghsatMGroup.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel), ReturnedState.None, "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 118);
						list = (await Server.Core.Assistance.Initializer.Dal.AghsatMGroup.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 124);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 127);
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 128);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionsAsync(List<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (partitions == null)
			{
				goto IL_004b;
			}
			if (partitions != null && partitions.Count <= 0)
			{
				goto IL_004b;
			}
			List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
			ReturnedSaveFuncInfo returnedSaveFuncInfo;
			foreach (short partition in partitions)
			{
				tasks.Add(SyncPartitionAsync(partition, connection));
				if (tasks.Count >= maxParalel)
				{
					Task<ReturnedSaveFuncInfo> t = await Task.WhenAny(tasks);
					returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 149);
					tasks.Remove(t);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 153);
			goto end_IL_0029;
			IL_004b:
			return ret;
			end_IL_0029:;
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 155);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 158);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 159);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 170);
				}
				if (partition <= 0)
				{
					partition = 1;
				}
				List<WebAghsatMGroup> lst = await Server.Core.Assistance.Initializer.Dal.AghsatMGroup.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
				lst = await ApplyAccessAsync(lst, connection);
				if (lst?.Any() ?? false)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 176);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.AghsatMGroup, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebAghsatMGroup.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 181);
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.AghsatMGroup, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 184);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 187);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 190);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsAghsatMGroup.cs", 191);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
