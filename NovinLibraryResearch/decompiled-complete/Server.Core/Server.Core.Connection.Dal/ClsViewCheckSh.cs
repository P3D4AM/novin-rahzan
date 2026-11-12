using System;
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
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsViewCheckSh : IEntity<WebViewCheckSh>
{
	private class NestedViewCheckSh
	{
		internal static readonly ClsViewCheckSh ViewCheckShInstance;

		static NestedViewCheckSh()
		{
			ViewCheckShInstance = new ClsViewCheckSh();
		}
	}

	public static ClsViewCheckSh ViewCheckShInstance => NestedViewCheckSh.ViewCheckShInstance;

	private ClsViewCheckSh()
	{
	}

	public async Task<List<WebViewCheckSh>> ApplyAccessAsync(List<WebViewCheckSh> lst, ClsConnectionData connection)
	{
		return await ApplyAccessAsync(lst, await connection.GetUserAccInfoAsync(), connection.Source);
	}

	public async Task<List<WebViewCheckSh>> ApplyAccessAsync(IEnumerable<WebViewCheckSh> lst, IClsConnectionData connection)
	{
		_ = 1;
		try
		{
			if (!connection.IsAccountingFamily)
			{
				return null;
			}
			return await ApplyAccessAsync(lst, await connection.GetUserAccInfoAsync(), connection.Source);
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 34);
			return null;
		}
	}

	public async Task<List<WebViewCheckSh>> ApplyAccessAsync(IEnumerable<WebViewCheckSh> lst, IUserAcc user, ENSource source)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs"))
		{
			try
			{
				if (source != ENSource.Accounting)
				{
					lst?.Count();
					lst = lst?.Where((WebViewCheckSh p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return lst?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 55);
				return null;
			}
		}
	}

	internal async Task ViewCheckShInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebViewCheckSh> e)
	{
		using (new JaegerMonitor("ViewCheckShInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ViewCheckSh, 1, isReceivedPacket: true);
				line = 1;
				ConnectionEventArgs<WebViewCheckSh> connectionEventArgs = e;
				if (connectionEventArgs != null && connectionEventArgs.ConnectionData.Count <= 0)
				{
					return;
				}
				line = 2;
				new List<SaveResponse>();
				line = 3;
				List<SaveResponse> list = await Server.Core.Assistance.Initializer.Dal.ViewCheckSh.SaveAsync(e.Connection.DepId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
				line = 4;
				foreach (SaveResponse item in list)
				{
					line = 5;
					item.PartitionId = e.PartitionId;
					line = 6;
				}
				line = 10;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 11;
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebViewCheckSh.GenerateSaveResponse(list), Priority.High);
				}
				line = 15;
				List<WebViewCheckSh> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				line = 20;
				List<ClsConnectionData> list2 = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting) && p?.ClientVersion >= 7).ToList();
				line = 25;
				if (list2 != null)
				{
					foreach (ClsConnectionData Connection in list2)
					{
						if (Connection != null && !Connection.IsDisposed)
						{
							List<WebViewCheckSh> list3 = await ApplyAccessAsync(e?.ConnectionData, Connection);
							if (list3 != null && list3.Any() && !(Connection?.IsDisposed ?? true))
							{
								Connection.SendPacketAsync(WebViewCheckSh.GenerateReadResponse(list3, 0), Priority.Medium);
							}
						}
					}
				}
				line = 30;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "ViewCheckShInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 124);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebViewCheckSh.GenerateSaveResponse(responses), Priority.High);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 140);
				}
				SyncInformation syncInformation = await connection.GetSyncInfoAsync();
				if (syncInformation == null)
				{
					ret.AddReturnedValue(ReturnedState.Error, "TaskHas been Canceled!", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 142);
					return ret;
				}
				if (syncInformation.ViewCheckShReset)
				{
					IEnumerable<WebViewCheckSh> enumerable = await ApplyAccessAsync((IEnumerable<WebViewCheckSh>)(await Server.Core.Assistance.Initializer.Dal.ViewCheckSh.GetAllAsync(connection.DepId, connection.cancellationToken)), (IClsConnectionData)connection);
					List<SyncResetItem> ResetItems = new List<SyncResetItem>();
					if (enumerable != null && enumerable.Count() > 0)
					{
						foreach (WebViewCheckSh item in enumerable)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.ViewCheckSh, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.ViewCheckSh, ResetItems), Priority.High));
					if (await waiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.ViewCheckSh, connection.cancellationToken);
					}
				}
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.ViewCheckSh.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 181);
							return ret;
						}
						await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
						list = (await Server.Core.Assistance.Initializer.Dal.ViewCheckSh.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 190);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 193);
				ret.AddReturnedValue(ex2, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 194);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"CheckShSyncResult:{!ret.HasError}");
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 221);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 232);
				}
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				IEnumerable<WebViewCheckSh> lst = await (Server.Core.Assistance.Initializer.Dal?.ViewCheckSh?.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken));
				IEnumerable<WebViewCheckSh> deletedLst = lst;
				lst = await ApplyAccessAsync(lst, connection);
				if (lst == null)
				{
					lst = new List<WebViewCheckSh>();
				}
				deletedLst = deletedLst?.Except(lst)?.ToList();
				if (deletedLst?.Any() ?? false)
				{
					deletedLst = deletedLst.Where((WebViewCheckSh p) => p.ResetTime > syncInfo.ViewCheckSh).ToList();
					IEnumerable<WebViewCheckSh> enumerable = deletedLst;
					if (enumerable != null && enumerable.Count() > 0)
					{
						GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, partition, 10000, connection.cancellationToken, EntityPart.ViewCheckSh, connection.ContextGuid);
						if (connection.Source == ENSource.WebsiteWordPress)
						{
							Task.Run(() => connection.SendPacketAsync(WebViewCheckSh.GenerateDeleteRequest(deletedLst, partition), Priority.Low));
						}
						else
						{
							Task.Run(() => connection.SendPacketAsync(WebViewCheckSh.GenerateDeleteRequest(deletedLst.Select((WebViewCheckSh p) => p.Guid), partition), Priority.Low));
						}
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 258);
						}
						if (!(await getDeleteResponsePacket.WaitForDeleteDonePacket()) && Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
						{
							ret.AddReturnedValue(ReturnedState.Error, "DeleteWaiter TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 262);
							return ret;
						}
					}
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 273);
				}
				if (lst?.Any() ?? false)
				{
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.ViewCheckSh, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebViewCheckSh.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 280);
						return ret;
					}
					ret.throwExceptionIfError();
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.ViewCheckSh, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 284);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 287);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 290);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckSh.cs", 291);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
