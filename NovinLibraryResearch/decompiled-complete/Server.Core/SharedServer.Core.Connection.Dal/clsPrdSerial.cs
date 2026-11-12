using System;
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
using Server.Core.Connection;
using Server.Core.Connection.Dal;

namespace SharedServer.Core.Connection.Dal;

public class clsPrdSerial
{
	private class NestedSerial
	{
		internal static readonly clsPrdSerial SerialInstance;

		static NestedSerial()
		{
			SerialInstance = new clsPrdSerial();
		}
	}

	public static clsPrdSerial Instance => NestedSerial.SerialInstance;

	public Task<List<WebPrdSerial>> ApplyAccessAsync(IEnumerable<WebPrdSerial> lst, IClsConnectionData connection)
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
				lst = lst?.Where((WebPrdSerial p) => p.ServerStatus == ServerStatusForClient.DirectDelivery || p.ServerStatus == ServerStatusForClient.Delivered);
			}
			return Task.FromResult(lst?.ToList());
		}
		catch (Exception ex)
		{
			WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 43);
			throw;
		}
	}

	internal async Task Instance_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdSerial> e)
	{
		using (new JaegerMonitor("Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.PrdSerial, 1, isReceivedPacket: true);
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.Serial.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.Connection.Source, e.ConnectionData, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
					foreach (SaveResponse item in ret)
					{
						item.PartitionId = e.PartitionId;
					}
				}
				catch (OperationCanceledException)
				{
					return;
				}
				catch (Exception ex2)
				{
					WebErrorLog.Instance.StartLog(ex2, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 65);
					ret = ret.Append(new SaveResponse
					{
						SaveResultStatus = false
					});
					result = false;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, result))
				{
					ClsConnectionData connection = ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
					if (!(connection?.IsDisposed ?? true))
					{
						await connection.SendPacketAsync(WebPrdSerial.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (trueSaved?.Any() ?? false)
				{
					IEnumerable<WebPrdSerial> enumerable = e.ConnectionData.Where((WebPrdSerial p) => trueSaved.Contains(p.Guid));
					if (enumerable != null && enumerable != null && enumerable.Count() > 0)
					{
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "Instance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 84);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2ClientAsync(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 93);
				}
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				if (connection?.IsDisposed ?? true)
				{
					ret.AddReturnedValue(ReturnedState.Error, "connection is disposed", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 96);
					return ret;
				}
				if ((object)(await connection.GetSyncInfoAsync()) == null)
				{
					throw new Exception("connection.SyncInfo is null");
				}
				List<short> totalPartitions = new List<short>();
				List<short> list = await Server.Core.Assistance.Initializer.Dal.Serial.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken);
				if (list != null && list.Any())
				{
					totalPartitions.AddRange(list);
					while (list != null && list.Any())
					{
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							ret.AddReturnedValue(ReturnedState.Error, "connection is disposed or cancellRequsted", "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 111);
							return ret;
						}
						await SyncPartitionsAsync(list, connection, ConnectionUniqueId, maxParalel);
						list = (await Server.Core.Assistance.Initializer.Dal.Serial.GetPartitionsAsync(connection.DepId, connection.HashId, connection.cancellationToken))?.Where((short x) => totalPartitions != null && !totalPartitions.Contains(x))?.ToList();
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 122);
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 125);
				WebErrorLog.Instance.StartLog(ex, "", "Sync2ClientAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 126);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
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
			WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionsAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 151);
		}
		return false;
	}

	private async Task<ReturnedSaveFuncInfo> SyncPartitionAsync(short partition, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				DateTime syncing = DateTime.Now;
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 161);
				}
				List<WebPrdSerial> lst = await Server.Core.Assistance.Initializer.Dal.Serial.GetAllAsync(connection.DepId, connection.HashId, partition, connection.cancellationToken);
				await connection.GetSyncInfoAsync();
				lst = await ApplyAccessAsync(lst, connection);
				if (lst?.Any() ?? false)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 168);
					}
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.PrdSerial, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(WebPrdSerial.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
					if (!(await waiter.WaitForSyncDonePacket()))
					{
						ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket timeOut", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 173);
						return ret;
					}
					ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
					returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.PrdSerial, syncing, connection), ReturnedState.None, "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 176);
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 179);
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 182);
				ret.AddReturnedValue(ex2, "", "SyncPartitionAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\clsPrdSerial.cs", 183);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}
}
