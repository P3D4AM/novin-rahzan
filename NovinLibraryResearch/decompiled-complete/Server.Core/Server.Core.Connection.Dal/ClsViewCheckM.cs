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

public class ClsViewCheckM : IEntity<WebViewCheckM>
{
	private class NestedViewCheckM
	{
		internal static readonly ClsViewCheckM ViewCheckMInstance;

		static NestedViewCheckM()
		{
			ViewCheckMInstance = new ClsViewCheckM();
		}
	}

	public static ClsViewCheckM ViewCheckMInstance => NestedViewCheckM.ViewCheckMInstance;

	private ClsViewCheckM()
	{
	}

	public async Task<List<WebViewCheckM>> ApplyAccessAsync(List<WebViewCheckM> lst, ClsConnectionData connection, CancellationToken token)
	{
		return await ApplyAccessAsync(lst, await connection.GetUserAccInfoAsync(), connection.IsAccountingFamily, connection.Source);
	}

	public async Task<List<WebViewCheckM>> ApplyAccessAsync(IEnumerable<WebViewCheckM> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		return await ApplyAccessAsync(lst, await connection.GetUserAccInfoAsync(), connection.IsAccountingFamily, connection.Source);
	}

	public async Task<List<WebViewCheckM>> ApplyAccessAsync(IEnumerable<WebViewCheckM> lst, IUserAcc user, bool isAccountingFamily, ENSource source)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs"))
		{
			try
			{
				if (!isAccountingFamily)
				{
					return null;
				}
				if (source != ENSource.Accounting)
				{
					lst?.Count();
					lst = lst?.Where((WebViewCheckM p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
				}
				return lst?.ToList();
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 46);
				return null;
			}
		}
	}

	internal async Task ViewCheckMInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebViewCheckM> e)
	{
		using (new JaegerMonitor("ViewCheckMInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs"))
		{
			int line = 0;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.ViewCheckM, 1, isReceivedPacket: true);
				line = 1;
				if (e?.ConnectionData == null || !e.ConnectionData.Any())
				{
					return;
				}
				line = 2;
				new List<SaveResponse>();
				line = 3;
				List<SaveResponse> list = await Server.Core.Assistance.Initializer.Dal.ViewCheckM.SaveAsync(e.Connection.DepId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
				line = 4;
				foreach (SaveResponse item in list)
				{
					line = 5;
					item.PartitionId = e.PartitionId;
					line = 6;
				}
				line = 7;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 8;
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebViewCheckM.GenerateSaveResponse(list), Priority.High);
					line = 9;
				}
				line = 10;
				List<WebViewCheckM> connectionData = e.ConnectionData;
				if (connectionData != null && connectionData.Count > 0)
				{
					line = 11;
					List<ClsConnectionData> connectionsData = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId);
					line = 12;
					connectionsData = connectionsData?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && (p == null || p.Source != ENSource.Accounting) && p?.ClientVersion >= 7 && p.IsAccountingFamily).ToList();
					line = 130;
					if (connectionsData != null)
					{
						foreach (ClsConnectionData Connection in connectionsData)
						{
							line = 131;
							if (Connection?.IsDisposed ?? true)
							{
								continue;
							}
							List<WebViewCheckM> dataForSend = await ApplyAccessAsync(e.ConnectionData, Connection);
							line = 132;
							if (dataForSend?.Any() ?? false)
							{
								Task.Run(() => Connection.SendPacketAsync(WebViewCheckM.GenerateReadResponse(dataForSend, 0), Priority.Medium));
							}
						}
					}
				}
				line = 20;
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "ViewCheckMInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 117);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebViewCheckM.GenerateSaveResponse(responses), Priority.High);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 134);
				}
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "disposed", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 135);
					return ret;
				}
				SyncInformation obj = await connection.GetSyncInfoAsync();
				if (obj == null)
				{
					throw new NullReferenceException("connection.SyncInfo is null|!");
				}
				if (obj.ViewCheckMReset)
				{
					IEnumerable<WebViewCheckM> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.ViewCheckM.GetAllAsync(connection.DepId, connection.cancellationToken), connection);
					List<SyncResetItem> ResetItems = new List<SyncResetItem>();
					if (enumerable != null && enumerable.Count() > 0)
					{
						foreach (WebViewCheckM item in enumerable)
						{
							ResetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.ViewCheckM, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.ViewCheckM, ResetItems), Priority.High));
					if (await waiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.ViewCheckM, connection.cancellationToken);
					}
				}
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.ViewCheckM.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 163);
							return ret;
						}
						await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
						list = (await Server.Core.Assistance.Initializer.Dal.ViewCheckM.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 172);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 175);
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 176);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"CheckMSyncResult:{!ret.HasError}");
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 202);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				DateTime syncing = DateTime.Now;
				line = 1;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 215);
				}
				line = 2;
				SyncInformation syncInfo = await connection.GetSyncInfoAsync();
				line = 3;
				IEnumerable<WebViewCheckM> lst = await Server.Core.Assistance.Initializer.Dal.ViewCheckM.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
				line = 4;
				IEnumerable<WebViewCheckM> deletedLst = lst;
				line = 5;
				lst = await ApplyAccessAsync(lst, connection);
				line = 6;
				if (lst == null)
				{
					lst = new List<WebViewCheckM>();
				}
				deletedLst = deletedLst?.Except(lst)?.ToList();
				line = 7;
				if (deletedLst?.Any() ?? false)
				{
					line = 8;
					deletedLst = deletedLst.Where((WebViewCheckM p) => p.ResetTime > syncInfo.ViewCheckM).ToList();
					line = 9;
					IEnumerable<WebViewCheckM> enumerable = deletedLst;
					if (enumerable != null && enumerable.Count() > 0)
					{
						line = 10;
						GetDeleteResponsePacket getDeleteResponsePacket = new GetDeleteResponsePacket(connection.HashId, partition, 10000, connection.cancellationToken, EntityPart.ViewCheckM, connection.ContextGuid);
						line = 11;
						if (connection.Source == ENSource.WebsiteWordPress)
						{
							line = 12;
							Task.Run(() => connection.SendPacketAsync(WebViewCheckM.GenerateDeleteRequest(deletedLst, partition), Priority.Low));
						}
						else
						{
							line = 13;
							Task.Run(() => connection.SendPacketAsync(WebViewCheckM.GenerateDeleteRequest(deletedLst.Select((WebViewCheckM p) => p.Guid).ToList(), partition), Priority.Low));
						}
						line = 14;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 257);
						}
						if (!(await getDeleteResponsePacket.WaitForDeleteDonePacket()))
						{
							line = 15;
							if (Server.Core.Assistance.Initializer.Dal.ResetTimeFlag)
							{
								line = 16;
								ret.AddReturnedValue(ReturnedState.Error, "DeleteWaiter TimeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 264);
								line = 17;
								return ret;
							}
						}
					}
				}
				line = 18;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 277);
				}
				line = 19;
				if (lst?.Any() ?? false)
				{
					line = 20;
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.ViewCheckM, 300000, connection.cancellationToken);
					line = 21;
					Task.Run(() => connection.SendPacketAsync(WebViewCheckM.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
					line = 22;
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						line = 23;
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 289);
						line = 24;
						return ret;
					}
					line = 25;
					ret.throwExceptionIfError();
					line = 26;
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.ViewCheckM, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 296);
					line = 27;
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 300);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"#Line_{line}", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 303);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsViewCheckM.cs", 304);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
