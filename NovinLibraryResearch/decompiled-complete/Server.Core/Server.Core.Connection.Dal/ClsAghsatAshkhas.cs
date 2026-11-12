using System;
using System.Collections.Concurrent;
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

public class ClsAghsatAshkhas : IEntity<WebAghsatAshkhas>
{
	private class NestedAghsatAshkhas
	{
		internal static readonly ClsAghsatAshkhas AghsatAshkhasInstance;

		static NestedAghsatAshkhas()
		{
			AghsatAshkhasInstance = new ClsAghsatAshkhas();
		}
	}

	private static ConcurrentDictionary<short, bool> dic_IsNewAghsatAshkhasExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsAghsatAshkhas AghsatAshkhasInstance => NestedAghsatAshkhas.AghsatAshkhasInstance;

	private ClsAghsatAshkhas()
	{
	}

	public async Task<List<WebAghsatAshkhas>> ApplyAccessAsync(IEnumerable<WebAghsatAshkhas> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebAghsatAshkhas p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	public async Task<IEnumerable<WebAghsatAshkhas>> ApplyAccessAsync(IEnumerable<WebAghsatAshkhas> lst, Guid token)
	{
		return lst;
	}

	internal async Task<ReturnedSaveFuncInfo> AghsatAshkhasInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebAghsatAshkhas> e)
	{
		using (new JaegerMonitor("AghsatAshkhasInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				if (!(e.ConnectionData?.Any() ?? false))
				{
					throw new NullReferenceException("e.connectionData has no data!");
				}
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.AghsatAshkhas, 1, isReceivedPacket: true);
				line = 1;
				if (e.Connection == null || e.Connection.IsDisposed || e.Connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "AghsatAshkhasInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 65);
				}
				line = 2;
				ConnectionEventArgs<WebAghsatAshkhas> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return ret;
				}
				line = 3;
				IEnumerable<SaveResponse> lctSaveResponses = new List<SaveResponse>();
				bool result = true;
				line = 4;
				try
				{
					line = 5;
					lctSaveResponses = await Server.Core.Assistance.Initializer.Dal.AghsatAshkhas.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					line = 6;
					foreach (SaveResponse item in lctSaveResponses)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					ret.AddErrorTokenIsCancelled("", "", "AghsatAshkhasInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 81);
					return ret;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "AghsatAshkhasInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 84);
					line = 7;
					lctSaveResponses = lctSaveResponses.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					line = 8;
					result = false;
				}
				line = 10;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					line = 13;
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					line = 14;
					if (connection != null)
					{
						await connection.SendPacketAsync(WebAghsatAshkhas.GenerateSaveResponse(lctSaveResponses), Priority.High);
					}
				}
				line = 15;
				ConnectionEventArgs<WebAghsatAshkhas> connectionEventArgs2 = e;
				if (connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting)
				{
					ConnectionEventArgs<WebAghsatAshkhas> connectionEventArgs3 = e;
					if (connectionEventArgs3 != null && connectionEventArgs3.ConnectionData?.Count > 0)
					{
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebAghsatAshkhas.GenerateSaveRequest(e.ConnectionData, 0, $"ارسال بسته دریافتی از سورس {(e?.Connection?.Source).GetValueOrDefault()} (HashId:{(e?.Connection?.HashId).GetValueOrDefault()}) به حسابداری"), Priority.Medium);
					}
				}
				line = 20;
				List<WebAghsatAshkhas> connectionData = e.ConnectionData;
				if (connectionData != null && connectionData.Count > 0)
				{
					line = 21;
					List<ClsConnectionData> connectionsData = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId);
					line = 22;
					connectionsData = connectionsData?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting)).ToList();
					line = 230;
					if (connectionsData != null)
					{
						line = 231;
						foreach (ClsConnectionData Connection in connectionsData)
						{
							line = 232;
							if (Connection == null || Connection.IsDisposed)
							{
								continue;
							}
							line = 2330;
							List<WebAghsatAshkhas> list = await ApplyAccessAsync(e.ConnectionData, Connection);
							line = 23310;
							if (list != null && list.Any())
							{
								line = 23311;
								List<Packet> packs = WebAghsatAshkhas.GenerateReadResponse(list, 0);
								line = 2332;
								if (Connection?.IsDisposed ?? true)
								{
									return ret;
								}
								line = 2333;
								Connection.SendPacketAsync(packs, Priority.Medium);
							}
						}
					}
				}
			}
			catch (Exception ex3)
			{
				ret.AddReturnedValue(ex3, "", "AghsatAshkhasInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 140);
				WebErrorLog.Instance.StartLog(ex3, $"#Line_{line}", "AghsatAshkhasInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 141);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebAghsatAshkhas.GenerateSaveResponse(responses), Priority.High);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				connection?.cancellationToken.ThrowIfCancellationRequested();
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection isDisposed or SyncInfo is null!!", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 216);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				SyncInformation obj = await connection.GetSyncInfoAsync();
				if (obj == null)
				{
					throw new NullReferenceException("connection.SyncInfo is null!!");
				}
				if (obj.AghsatAshkhasReset)
				{
					IEnumerable<WebAghsatAshkhas> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.AghsatAshkhas.GetAllAsync(connection.DepId, connection.cancellationToken), connection);
					List<SyncResetItem> ResetItems = new List<SyncResetItem>();
					if (enumerable != null && enumerable.Count() > 0)
					{
						foreach (WebAghsatAshkhas item in enumerable)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.AghsatAshkhas, 60000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.AghsatAshkhas, ResetItems), Priority.High));
					if (await waiter.WaitForSyncDonePacket())
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.AghsatAshkhas, connection.cancellationToken), ReturnedState.None, "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 236);
					}
					else
					{
						ret.AddError("timedOut on AghsatAshKhas sync", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 239);
					}
				}
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.AghsatAshkhas.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
						returnedSaveFuncInfo.AddReturnedValue(await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel), ReturnedState.None, "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 251);
						list = (await Server.Core.Assistance.Initializer.Dal.AghsatAshkhas.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 257);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 260);
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 261);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"ShestAshkhasSyncResult:{!ret.HasError}");
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
					returnedSaveFuncInfo.AddReturnedValue(await t, ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 284);
					tasks.Remove(t);
				}
			}
			returnedSaveFuncInfo = ret;
			returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 288);
			goto end_IL_0029;
			IL_004b:
			return ret;
			end_IL_0029:;
		}
		catch (OperationCanceledException exception)
		{
			ret.AddReturnedValue(exception, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 290);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 293);
			ret.AddReturnedValue(ex, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 294);
		}
		return ret;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 306);
				}
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				IEnumerable<WebAghsatAshkhas> lst = await Server.Core.Assistance.Initializer.Dal.AghsatAshkhas.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
				IEnumerable<WebAghsatAshkhas> deletedLst = lst;
				lst = await ApplyAccessAsync(lst, connection);
				deletedLst = deletedLst?.Except(lst).ToList();
				IEnumerable<WebAghsatAshkhas> enumerable = deletedLst;
				if (enumerable != null && enumerable.Count() > 0)
				{
					deletedLst = deletedLst?.Where((WebAghsatAshkhas p) => p.ResetTime > syncInfo.AghsatAshkhas && (p.ServerStatus == ServerStatusForClient.Delivered || p.ServerStatus == ServerStatusForClient.DirectDelivery)).ToList();
					IEnumerable<WebAghsatAshkhas> enumerable2 = deletedLst;
					if (enumerable2 != null && enumerable2.Count() > 0)
					{
						GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, partition, 10000, connection.cancellationToken, EntityPart.AghsatAshkhas, connection.ContextGuid);
						if (connection.Source == ENSource.WebsiteWordPress)
						{
							Task.Run(() => connection.SendPacketAsync(WebAghsatAshkhas.GenerateDeleteRequest(deletedLst, partition), Priority.Low));
						}
						else
						{
							Task.Run(() => connection.SendPacketAsync(WebAghsatAshkhas.GenerateDeleteRequest(deletedLst.Select((WebAghsatAshkhas p) => p.Guid), partition), Priority.Low));
						}
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 331);
						}
						if (!(await getDeleteResponsePacket.WaitForDeleteDonePacket()) && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
						{
							ret.AddReturnedValue(ReturnedState.Error, "DeleteWaiter TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 335);
							return ret;
						}
					}
				}
				if (lst?.Any() ?? false)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 348);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.AghsatAshkhas, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebAghsatAshkhas.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 353);
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.AghsatAshkhas, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 356);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 359);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 362);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 363);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs"))
		{
			_ = 2;
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.AghsatAshkhas, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 412);
				}
				if (list == null || !list.Any())
				{
					dic_IsNewAghsatAshkhasExistsFromClient.TryRemove(connection.DepId, out var _);
					dic_IsNewAghsatAshkhasExistsFromClient.TryAdd(connection.DepId, value: false);
					return ret.AddInformation("اقساط دریافتنی از شخاص برای ارسال به حسابداری موجود نیست.", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 417);
				}
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال قسط دریافتنی از اشخاص({item})", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 422);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 423);
					}
					WebAghsatAshkhas webAghsatAshkhas = await Server.Core.Assistance.Initializer.Dal.AghsatAshkhas.GetAsync(item, connection.DepId);
					if (webAghsatAshkhas != null)
					{
						await connection.SendPacketAsync(WebAghsatAshkhas.GenerateSaveRequest(new List<WebAghsatAshkhas> { webAghsatAshkhas }, 0, "سینک با حسابداری"), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 431);
				throw;
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 436);
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatAshkhas.cs", 437);
			}
			return ret;
		}
	}
}
