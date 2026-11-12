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
using PacketParser.TransferLayer.Waiters.Accounting.Hesab;
using Serilog;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal;

public class ClsPrdAnbarGardani : IEntity<WebPrdAnbarGardani>
{
	private class NestedPrdAnbarGardani
	{
		internal static readonly ClsPrdAnbarGardani PrdAnbarGardaniInstance;

		static NestedPrdAnbarGardani()
		{
			PrdAnbarGardaniInstance = new ClsPrdAnbarGardani();
		}
	}

	private static ConcurrentDictionary<short, DateTime> _dicIsModifiedDepartmentData = new ConcurrentDictionary<short, DateTime>();

	private static ConcurrentDictionary<short, bool> _dicIsNewPrdAnbarGardaniExistsFromClient = new ConcurrentDictionary<short, bool>();

	public static ClsPrdAnbarGardani PrdAnbarGardaniInstance => NestedPrdAnbarGardani.PrdAnbarGardaniInstance;

	public static void UpdateDictionary(short depId)
	{
		_dicIsModifiedDepartmentData.TryRemove(depId, out var _);
		_dicIsModifiedDepartmentData.TryAdd(depId, DateTime.Now);
	}

	public Task<List<WebPrdAnbarGardani>> ApplyAccessAsync(IEnumerable<WebPrdAnbarGardani> lst, IClsConnectionData connection)
	{
		if (!connection.IsAccountingFamily)
		{
			return Task.FromResult<List<WebPrdAnbarGardani>>(null);
		}
		return Task.FromResult(lst?.ToList());
	}

	internal async Task PrdAnbarGardaniInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebPrdAnbarGardani> e)
	{
		using (new JaegerMonitor("PrdAnbarGardaniInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Sanad, 1, isReceivedPacket: true);
				if (e != null && e.ConnectionData.Count <= 0)
				{
					return;
				}
				IEnumerable<SaveResponse> ret = new List<SaveResponse>();
				bool result = true;
				try
				{
					ret = await Server.Core.Assistance.Initializer.Dal.PrdAnbarGardani.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
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
					WebErrorLog.Instance.StartLog(ex2, "", "PrdAnbarGardaniInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 55);
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
						await connection.SendPacketAsync(WebPrdAnbarGardani.GenerateSaveResponse(ret), Priority.High);
					}
				}
				HashSet<Guid> trueSaved = new HashSet<Guid>(ret?.Where((SaveResponse p) => p.SaveResultStatus)?.Select((SaveResponse p) => p.Guid));
				if (!(trueSaved?.Any() ?? false))
				{
					return;
				}
				IEnumerable<WebPrdAnbarGardani> enumerable = e.ConnectionData.Where((WebPrdAnbarGardani p) => trueSaved.Contains(p.Guid));
				if (enumerable == null || (enumerable != null && enumerable.Count() <= 0))
				{
					return;
				}
				if ((e == null || e.Connection.Source != ENSource.Accounting) && enumerable != null && enumerable.Any())
				{
					ClsConnectionData accountingConnection = ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId);
					if (accountingConnection != null && accountingConnection?.ClientVersion >= 10)
					{
						accountingConnection?.SendPacketAsync(WebPrdAnbarGardani.GenerateSaveRequest(e.ConnectionData, Guid.NewGuid()), Priority.High);
					}
				}
				if (e.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				throw new InvalidOperationException();
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex4)
			{
				WebErrorLog.Instance.StartLog(ex4, "", "PrdAnbarGardaniInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 85);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, short maxParalel = 10)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 1000;
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection is disposed", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 97);
				}
				line = 1001;
				if (connection.Source == ENSource.WebsiteWordPress)
				{
					return ret;
				}
				line = 1002;
				if ((await connection.GetSyncInfoAsync()).PrdAnbarGardaniReset)
				{
					line = 5;
					IEnumerable<WebPrdAnbarGardani> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.PrdAnbarGardani.GetAll(connection.DepId, DateTime.Now, default(CancellationToken)), connection);
					List<SyncResetItem> list = new List<SyncResetItem>();
					line = 10;
					if (enumerable != null && enumerable.Any())
					{
						foreach (WebPrdAnbarGardani item in enumerable)
						{
							list.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					line = 20;
					using SyncDoneWaiter waiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.PrdAnbarGardani, 300000, connection.cancellationToken);
					connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.PrdAnbarGardani, list), Priority.High);
					line = 25;
					if (await waiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.PrdAnbarGardani, connection.cancellationToken);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 123);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 126);
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 127);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendUnsent(ClsConnectionData connection)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs"))
		{
			_ = 2;
			try
			{
				int millisecondsDelay = 180000;
				CancellationTokenSource tokenTimeOut = CancellationTokenSource.CreateLinkedTokenSource(connection.cancellationToken, new CancellationTokenSource(millisecondsDelay).Token);
				tokenTimeOut.CancelAfter(millisecondsDelay);
				List<Guid> list2 = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.PrdAnbarGardani, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 142);
				}
				if (list2 == null || !list2.Any())
				{
					return ret.AddInformation("انبارگردانی برای ارسال به حسابداری موجود نیست", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 143);
				}
				List<WebPrdAnbarGardani> list = new List<WebPrdAnbarGardani>();
				foreach (Guid item in list2)
				{
					ret.AddInformation($"در حال ارسال انبارگردانی({item}) به حسابداری.", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 147);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 148);
					}
					if (tokenTimeOut.IsCancellationRequested)
					{
						WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in PrdAnbarGardani !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 151);
						return ret;
					}
					WebPrdAnbarGardani webPrdAnbarGardani = await Server.Core.Assistance.Initializer.Dal.PrdAnbarGardani.LoadDataAsync(connection.DepId, item);
					if (webPrdAnbarGardani != null)
					{
						list.Add(webPrdAnbarGardani);
						if (tokenTimeOut.IsCancellationRequested)
						{
							WebErrorLog.Instance.StartLog("!!! TimeOut Token is Cancell in PrdAnbarGardani !!!", "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 159);
							return ret;
						}
					}
				}
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await SendAsync(list, connection), ReturnedState.None, "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 163);
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 165);
			}
			catch (Exception ex)
			{
				if (connection?.cancellationToken.IsCancellationRequested ?? true)
				{
					Serilog.Log.Warning(ex, "");
				}
				else
				{
					WebErrorLog.Instance.StartLog(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 171);
				}
				ret.AddReturnedValue(ex, "", "SendUnsent", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 172);
			}
			return ret;
		}
	}

	private async Task<ReturnedSaveFuncInfo> SendAsync(List<WebPrdAnbarGardani> list, ClsConnectionData connection)
	{
		using (new JaegerMonitor("SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				using DeliveryBatchWaiter waiter = new DeliveryBatchWaiter(connection.Context, Guid.NewGuid(), list.Select((WebPrdAnbarGardani q) => q.Guid)?.ToList(), EntityPart.PrdAnbarGardani, connection.cancellationToken);
				connection.SendPacketAsync(WebPrdAnbarGardani.GenerateSaveRequest(list, waiter.PacketRefrencedId), Priority.High);
				ReturnedSaveFuncInfo returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await waiter.WaitForDelivery(), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 184);
				returnedSaveFuncInfo = ret;
				returnedSaveFuncInfo.AddReturnedValue(await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(0, EntityPart.PrdAnbarGardani, DateTime.Now, connection), ReturnedState.None, "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 185);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 189);
				ret.AddReturnedValue(ex, "", "SendAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsPrdAnbarGardani.cs", 190);
			}
			return ret;
		}
	}
}
