using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.Entities.WordPress;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using PacketParser.TransferLayer;
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsCustomerRole : IEntity<WebCustomerRole>
{
	private class NestedCustomerRole
	{
		internal static readonly ClsCustomerRole CustomerRoleInstance;

		static NestedCustomerRole()
		{
			CustomerRoleInstance = new ClsCustomerRole();
		}
	}

	public static ClsCustomerRole CustomerRoleInstance => NestedCustomerRole.CustomerRoleInstance;

	public async Task<List<WebCustomerRole>> ApplyAccessAsync(IEnumerable<WebCustomerRole> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return null;
		}
		if (connection.Source != ENSource.Accounting)
		{
			lst?.Count();
			lst = lst?.Where((WebCustomerRole p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
		}
		return lst?.ToList();
	}

	internal async Task CustomerRoleInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebCustomerRole> e)
	{
		using (new JaegerMonitor("CustomerRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs"))
		{
			_ = 2;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.CustomerRole, 1, isReceivedPacket: true);
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.CustomerRole.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
				}
				catch (Exception ex2)
				{
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
					WebErrorLog.Instance.StartLog(ex2, "", "CustomerRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 59);
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (connection != null)
					{
						await connection.SendPacketAsync(WebCustomerRole.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebCustomerRole> truSavedInRet = e.ConnectionData.Where((WebCustomerRole p) => trueSaved.Contains(p.Guid));
				if (!(truSavedInRet?.Any() ?? false))
				{
					return;
				}
				if ((e == null || e.Connection.Source != ENSource.Accounting) && (truSavedInRet?.Any() ?? false))
				{
					ClsConnectionData accconnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accconnection != null && accconnection?.ClientVersion >= 16)
					{
						Task.Run(() => accconnection?.SendPacketAsync(WebCustomerRole.GenerateSaveRequest(truSavedInRet), Priority.Medium));
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				IEnumerable<ClsConnectionData> enumerable = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p != null && !p.IsDisposed && p.IsAccountingFamily && p.Source != ENSource.Accounting && p.ClientVersion >= 16);
				if (enumerable == null || !enumerable.Any())
				{
					return;
				}
				foreach (ClsConnectionData Connection in enumerable)
				{
					List<WebCustomerRole> dataForSend = await ApplyAccessAsync(truSavedInRet, Connection);
					if (dataForSend?.Any() ?? false)
					{
						Task.Run(() => Connection.SendPacketAsync(WebCustomerRole.GenerateReadResponse(dataForSend, isSync: false), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "CustomerRoleInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 98);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, StringBuilder sb, short maxParalel = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		if (connection.Source == ENSource.WebsiteWordPress || connection.ClientVersion < 16)
		{
			return ret;
		}
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs"))
		{
			_ = 3;
			try
			{
				if (connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 108);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 110);
					return ret;
				}
				await connection.GetSyncInfoAsync();
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 112);
					return ret;
				}
				List<short> list = await Server.Core.Assistance.Initializer.Dal.CustomerRole.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				List<short> totalPartitions = new List<short>();
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 123);
							return ret;
						}
						await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 129);
							return ret;
						}
						if (connection == null || connection.IsDisposed)
						{
							return ret;
						}
						list = (await Server.Core.Assistance.Initializer.Dal.CustomerRole.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 140);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 141);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
				sb.AppendLine($"CusRoleSyncResult:{!ret.HasError}");
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
						tasks.Add(syncPartitionAsync(partition, connection));
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 167);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> syncPartitionAsync(short partition, ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		try
		{
			if (connection.ClientVersion < 10)
			{
				throw new Exception("version" + connection.Version + " is less than 10!");
			}
			DateTime syncing = DateTime.Now;
			if (connection.cancellationToken.IsCancellationRequested)
			{
				return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 177);
			}
			List<WebCustomerRole> list = await Server.Core.Assistance.Initializer.Dal.CustomerRole.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
			using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.CustomerRole, 300000, connection.cancellationToken);
			if (list != null && list.Any())
			{
				connection.SendPacketAsync(WebCustomerRole.GenerateReadResponse(list, isSync: true, partition), Priority.Medium, partition);
				if (!(await waiter.WaitForSyncDonePacket()))
				{
					ret.AddReturnedValue(ReturnedState.Error, "GenerateReadResponse waiter.WaitForSyncDonePacket", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 186);
					return ret;
				}
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 189);
				}
				ret.throwExceptionIfError();
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.CustomerRole, syncing, connection), ReturnedState.None, "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 191);
			}
		}
		catch (OperationCanceledException)
		{
			ret.AddErrorTokenIsCancelled("", "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 194);
		}
		catch (Exception ex2)
		{
			if (ex2.Message.Contains("Operation cancelled by user"))
			{
				throw new OperationCanceledException();
			}
			WebErrorLog.Instance.StartLog(ex2, "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 198);
			ret.AddReturnedValue(ex2, "", "syncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 199);
		}
		finally
		{
			Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection.cancellationToken, connection));
		}
		return ret;
	}

	public async Task<ReturnedSaveFuncInfo> SendAsync(WebCustomerRole role, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			if (connection.ClientVersion < 16)
			{
				throw new Exception("version" + connection.Version + " is less than 16!");
			}
			try
			{
				using CustomerRoleWaiter waiter = new CustomerRoleWaiter(connection.Context, role.Guid, connection.cancellationToken);
				Task.Run(() => connection.SendPacketAsync(WebCustomerRole.GenerateSaveRequest(new List<WebCustomerRole> { role }), Priority.Medium));
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForSaveResponse("ClsCustomerRole.SendRoleAsync"), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 213);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 215);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 218);
				ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 219);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string connectionUniqueId, short maxParalelTasks = 5)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs"))
		{
			_ = 4;
			try
			{
				if (connection.Source != ENSource.Accounting)
				{
					return ret.AddInformation($"Source({connection.Source})!=accounting", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 229);
				}
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.CustomerRole, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 231);
				}
				if (list == null || !list.Any())
				{
					return ret;
				}
				List<Task<ReturnedSaveFuncInfo>> tasks = new List<Task<ReturnedSaveFuncInfo>>();
				ReturnedSaveFuncInfo returnedSaveFuncInfo;
				foreach (Guid item in list)
				{
					ret.AddInformation($"sending CusRole({item}) to accounting connecion", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 237);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 238);
					}
					WebCustomerRole webCustomerRole = await Server.Core.Assistance.Initializer.Dal.CustomerRole.GetAsync(connection.DepId, item);
					if (webCustomerRole != null)
					{
						tasks.Add(SendAsync(webCustomerRole, connection));
						if (tasks.Count >= maxParalelTasks)
						{
							Task<ReturnedSaveFuncInfo> task = await Task.WhenAny(tasks);
							tasks.Remove(task);
							returnedSaveFuncInfo = ret;
							returnedSaveFuncInfo.AddReturnedValue(await task, ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 246);
						}
					}
				}
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Task.WhenAll(tasks), ReturnedState.None, "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 249);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 253);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 258);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsCustomerRole.cs", 259);
			}
			return ret;
		}
	}
}
