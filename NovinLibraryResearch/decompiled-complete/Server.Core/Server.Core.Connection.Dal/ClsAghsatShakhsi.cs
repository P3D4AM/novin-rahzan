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

public class ClsAghsatShakhsi : IEntity<WebAghsatShakhsi>
{
	private class NestedAghsatShakhsi
	{
		internal static readonly ClsAghsatShakhsi AghsatShakhsiInstance;

		static NestedAghsatShakhsi()
		{
			AghsatShakhsiInstance = new ClsAghsatShakhsi();
		}
	}

	private static ConcurrentDictionary<short, bool> dic_IsNewAghsatShakhsiExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsAghsatShakhsi AghsatShakhsiInstance => NestedAghsatShakhsi.AghsatShakhsiInstance;

	private ClsAghsatShakhsi()
	{
	}

	public async Task<List<WebAghsatShakhsi>> ApplyAccessAsync(IEnumerable<WebAghsatShakhsi> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebAghsatShakhsi p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task AghsatShakhsiInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebAghsatShakhsi> e)
	{
		using (new JaegerMonitor("AghsatShakhsiInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.AghsatShakhsi, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebAghsatShakhsi> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				new List<SaveResponse>();
				line = 3;
				List<SaveResponse> list = await Server.Core.Assistance.Initializer.Dal.AghsatShakhsi.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
				line = 4;
				foreach (SaveResponse item in list)
				{
					item.PartitionId = e.PartitionId;
				}
				line = 10;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 11;
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebAghsatShakhsi.GenerateSaveResponse(list), Priority.High);
				}
				line = 12;
				ConnectionEventArgs<WebAghsatShakhsi> connectionEventArgs2 = e;
				if (connectionEventArgs2 == null || connectionEventArgs2.Connection.Source != ENSource.Accounting)
				{
					ConnectionEventArgs<WebAghsatShakhsi> connectionEventArgs3 = e;
					if (connectionEventArgs3 != null && connectionEventArgs3.ConnectionData?.Count > 0)
					{
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebAghsatShakhsi.GenerateSaveRequest(e.ConnectionData, 0), Priority.Medium);
					}
				}
				line = 20;
				List<WebAghsatShakhsi> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				line = 21;
				List<ClsConnectionData> connectionsData = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId);
				line = 22;
				connectionsData = connectionsData?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting)).ToList();
				line = 23;
				if (connectionsData == null)
				{
					return;
				}
				foreach (ClsConnectionData connection in connectionsData)
				{
					if (!(connection?.IsDisposed ?? true))
					{
						line = 300;
						List<WebAghsatShakhsi> items = await ApplyAccessAsync(e.ConnectionData, connection);
						line = 301;
						List<Packet> packet = WebAghsatShakhsi.GenerateReadResponse(items, 0);
						line = 302;
						Task.Run(() => connection.SendPacketAsync(packet, Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "AghsatShakhsiInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 107);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebAghsatShakhsi.GenerateSaveResponse(responses), Priority.High);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 125);
				}
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 126);
					return ret;
				}
				SyncInformation obj = await connection.GetSyncInfoAsync();
				if (obj == null)
				{
					throw new Exception("connection.SyncInfo is null");
				}
				if (obj.AghsatShakhsiReset)
				{
					List<WebAghsatShakhsi> list = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.AghsatShakhsi.GetAllAsync(connection.DepId, connection.cancellationToken), connection);
					List<SyncResetItem> ResetItems = new List<SyncResetItem>();
					if (list != null && list.Count() > 0)
					{
						foreach (WebAghsatShakhsi item in list)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.AghsatShakhsi, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.AghsatShakhsi, ResetItems), Priority.High));
					if (await waiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.AghsatShakhsi, connection.cancellationToken);
					}
				}
				List<short> totalPartitions = new List<short>();
				List<short> list2 = await Server.Core.Assistance.Initializer.Dal.AghsatShakhsi.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list2 != null && list2.Any())
				{
					totalPartitions.AddRange(list2);
					while (list2 != null && list2.Any())
					{
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 167);
							return ret;
						}
						await SyncPartitionsAsync(list2, connection, ConnectionUniqueId, maxParalel);
						list2 = (await Server.Core.Assistance.Initializer.Dal.AghsatShakhsi.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 178);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 181);
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 182);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"GhestShakhsiSyncResult:{!ret.HasError}");
			}
			return ret;
		}
	}

	private async Task<bool> SyncPartitionsAsync(List<short> partitions, ClsConnectionData connection, string ConnectionUniqueId, short maxParalel)
	{
		_ = 1;
		try
		{
			new CancellationTokenSource();
			if (partitions != null)
			{
				if (partitions == null || partitions.Count > 0)
				{
					List<Task> tasks = new List<Task>();
					foreach (short partition in partitions)
					{
						tasks.Add(SyncPartitionAsync(partition, connection));
						if (tasks.Count >= maxParalel)
						{
							List<Task> list = tasks;
							list.Remove(await Task.WhenAny(tasks));
						}
					}
					await Task.WhenAll(tasks);
					return true;
				}
			}
			return true;
		}
		catch (OperationCanceledException)
		{
		}
		catch (Exception ex2)
		{
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 208);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 219);
				}
				IEnumerable<WebAghsatShakhsi> lst = await Server.Core.Assistance.Initializer.Dal.AghsatShakhsi.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				IEnumerable<WebAghsatShakhsi> deletedLst = lst;
				lst = await ApplyAccessAsync(lst, connection);
				deletedLst = deletedLst?.Except(lst).ToList();
				IEnumerable<WebAghsatShakhsi> enumerable = deletedLst;
				if (enumerable != null && enumerable.Count() > 0)
				{
					deletedLst = deletedLst?.Where((WebAghsatShakhsi p) => p.ResetTime > syncInfo.AghsatShakhsi && (p.ServerStatus == ServerStatusForClient.Delivered || p.ServerStatus == ServerStatusForClient.DirectDelivery)).ToList();
					IEnumerable<WebAghsatShakhsi> enumerable2 = deletedLst;
					if (enumerable2 != null && enumerable2.Count() > 0)
					{
						GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, partition, 10000, connection.cancellationToken, EntityPart.AghsatShakhsi, connection.ContextGuid);
						if (connection.Source == ENSource.WebsiteWordPress)
						{
							connection.SendPacketAsync(WebAghsatShakhsi.GenerateDeleteRequest(deletedLst, partition), Priority.Low);
						}
						else
						{
							connection.SendPacketAsync(WebAghsatShakhsi.GenerateDeleteRequest(deletedLst.Select((WebAghsatShakhsi p) => p.Guid), partition), Priority.Low);
						}
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 243);
						}
						if (!(await getDeleteResponsePacket.WaitForDeleteDonePacket()) && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
						{
							ret.AddReturnedValue(ReturnedState.Error, "DeleteWaiter TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 247);
							return ret;
						}
					}
				}
				if (lst?.Any() ?? false)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 260);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.AghsatShakhsi, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebAghsatShakhsi.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 265);
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.AghsatShakhsi, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 268);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 271);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 274);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 275);
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
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.AghsatShakhsi, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 325);
				}
				if (list == null || (list != null && list.Count <= 0))
				{
					dic_IsNewAghsatShakhsiExistsFromClient.TryRemove(connection.DepId, out var _);
					dic_IsNewAghsatShakhsiExistsFromClient.TryAdd(connection.DepId, value: false);
					return ret.AddInformation("هیچ قسط پرداختنی شخصی ای برای ارسال به حسابداری موجود نیست!", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 330);
				}
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال قسط پرداختنی شخصی ({item})", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 335);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 336);
					}
					WebAghsatShakhsi webAghsatShakhsi = await Server.Core.Assistance.Initializer.Dal.AghsatShakhsi.GetAsync(item, connection.DepId);
					if (webAghsatShakhsi != null)
					{
						await connection.SendPacketAsync(WebAghsatShakhsi.GenerateSaveRequest(new List<WebAghsatShakhsi> { webAghsatShakhsi }, 0), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 344);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 349);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsAghsatShakhsi.cs", 350);
			}
			return ret;
		}
	}
}
