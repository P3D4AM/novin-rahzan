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

namespace Server.Core.Connection.Dal;

public class ClsVisit : IEntity<WebVisit>
{
	private class NestedVisit
	{
		internal static readonly ClsVisit VisitInstance;

		static NestedVisit()
		{
			VisitInstance = new ClsVisit();
		}
	}

	public static ClsVisit VisitInstance => NestedVisit.VisitInstance;

	private ClsVisit()
	{
	}

	public Task<List<WebVisit>> ApplyAccessAsync(IEnumerable<WebVisit> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return Task.FromResult<List<WebVisit>>(null);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 36);
				return Task.FromResult<List<WebVisit>>(null);
			}
		}
	}

	internal async Task VisitInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebVisit> e)
	{
		using (new JaegerMonitor("VisitInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs"))
		{
			_ = 1;
			try
			{
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.Visit, 1, isReceivedPacket: true);
				if (e == null || e?.ConnectionData == null || e.ConnectionData.Count <= 0 || e.Connection == null)
				{
					return;
				}
				_ = e.Connection.cancellationToken;
				if (e?.Connection?.DepId <= 0)
				{
					return;
				}
				new List<SaveResponse>();
				List<SaveResponse> list = await Server.Core.Assistance.Initializer.Dal.Visit.Save(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
				foreach (SaveResponse item in list)
				{
					item.PartitionId = e.PartitionId;
				}
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebVisit.GenerateSaveResponse(list), Priority.High);
				}
				IEnumerable<WebVisit> enumerable = e.ConnectionData.Where((WebVisit prd) => prd.Guid == Guid.Empty);
				if (enumerable != null && enumerable.Count() > 0)
				{
					throw new Exception("کالای دریافتی حاوی شناسه صفر یا منفی میباشد");
				}
				ConnectionEventArgs<WebVisit> connectionEventArgs = e;
				if (connectionEventArgs == null || connectionEventArgs.Connection.Source != ENSource.Accounting)
				{
					ConnectionEventArgs<WebVisit> connectionEventArgs2 = e;
					if (connectionEventArgs2 != null && connectionEventArgs2.ConnectionData?.Count > 0)
					{
						ConnectionManager.Instance.GetAccountingConnection(e.Connection.DepId)?.SendPacketAsync(WebVisit.GenerateSaveRequest(e.ConnectionData, e.PartitionId), Priority.Medium);
					}
				}
				ConnectionEventArgs<WebVisit> connectionEventArgs3 = e;
				if (connectionEventArgs3 == null || connectionEventArgs3.Connection.Source != ENSource.Accounting)
				{
					return;
				}
				List<WebVisit> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				List<ClsConnectionData> list2 = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId).ToList();
				if (list2 == null)
				{
					return;
				}
				foreach (ClsConnectionData item2 in list2?.Where((ClsConnectionData p) => p.Source != ENSource.Accounting))
				{
					ClsConnectionData clsConnectionData = item2;
					clsConnectionData.SendPacketAsync(WebVisit.GenerateReadResponse(await ApplyAccessAsync(e.ConnectionData, item2), 0), Priority.Medium);
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "VisitInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 85);
				List<SaveResponse> responses = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebVisit.GenerateSaveResponse(responses), Priority.High);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			int line = 0;
			try
			{
				line = 100;
				if (connection?.IsDisposed ?? true)
				{
					return ret.AddError("connection is disposed!", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 103);
				}
				SyncInformation obj = await connection.GetSyncInfoAsync();
				line = 101;
				if (obj?.VisitReset ?? true)
				{
					line = 5;
					IEnumerable<WebVisit> enumerable = await ApplyAccessAsync(await Server.Core.Assistance.Initializer.Dal.Visit.GetAllAsync(connection, -1), connection);
					List<SyncResetItem> resetItems = new List<SyncResetItem>();
					line = 10;
					if (enumerable != null && enumerable.Count() > 0)
					{
						foreach (WebVisit item in enumerable)
						{
							resetItems.Add(new SyncResetItem
							{
								Guid = item.Guid
							});
						}
					}
					line = 20;
					using SyncDoneWaiter syncDoneWaiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, 0, EntityPart.Visit, 300000, connection.cancellationToken);
					Task.Run(() => connection.SendPacketAsync(Sync.GenerateSyncReset(EntityPart.Visit, resetItems), Priority.High));
					line = 25;
					if (await syncDoneWaiter.WaitForSyncDonePacket())
					{
						await Server.Core.Assistance.Initializer.Dal.Sync.SetSyncDoneAsync(connection.DepId, connection.HashId, EntityPart.Visit, connection.cancellationToken);
					}
				}
				line = 30;
				List<short> list = await Server.Core.Assistance.Initializer.Dal.Visit.GetPartitionsAsync(connection.DepId, connection.cancellationToken);
				if (list != null && list.Count > 0)
				{
					foreach (short partition in list)
					{
						line = 35;
						if (connection == null || connection.IsDisposed || connection.cancellationToken.IsCancellationRequested)
						{
							return ret;
						}
						DateTime syncing = DateTime.Now;
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 134);
						}
						IEnumerable<WebVisit> lst = await Server.Core.Assistance.Initializer.Dal.Visit.GetAllAsync(connection, partition);
						line = 40;
						lst = await ApplyAccessAsync(lst, connection);
						line = 50;
						if (!(lst?.Any() ?? false))
						{
							continue;
						}
						if (connection.cancellationToken.IsCancellationRequested)
						{
							return ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 148);
						}
						using SyncDoneWaiter syncDoneWaiter = new SyncDoneWaiter(connection.ContextGuid, connection.HashId, partition, EntityPart.Visit, 300000, connection.cancellationToken);
						Task.Run(() => connection.SendPacketAsync(WebVisit.GenerateReadResponse(lst, partition, isSync: true), Priority.Low));
						line = 55;
						if (!(await syncDoneWaiter.WaitForSyncDonePacket()))
						{
							line = 56;
							ret.AddReturnedValue(ReturnedState.Error, "WaitForSyncDonePacket error", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 155);
							return ret;
						}
						line = 60;
						ret.throwExceptionIfError();
						await Server.Core.Assistance.Initializer.Dal.Sync.SaveSyncedAsync(partition, EntityPart.Visit, syncing, connection);
					}
				}
			}
			catch (OperationCanceledException)
			{
				ret.AddErrorTokenIsCancelled("", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 164);
			}
			catch (Exception ex2)
			{
				ret.AddReturnedValue(ex2, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 167);
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 168);
			}
			finally
			{
				ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection);
			}
			return ret;
		}
	}

	public async Task<ReturnedSaveFuncInfo> SendNotDeliveredAsync(ClsConnectionData connection, string ConnectionUniqueId)
	{
		ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs"))
		{
			_ = 2;
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.Visit, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested)
				{
					return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 225);
				}
				if (list == null || !list.Any())
				{
					return ret.AddInformation("دلیوری جدیدی برای سینک با سرور موجود نمیباشد", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 227);
				}
				foreach (Guid item in list)
				{
					ret.AddInformation($"در حال ارسال ویزیت به حسابداری Guid({item})", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 232);
					if (connection.cancellationToken.IsCancellationRequested)
					{
						return ret.AddErrorTokenIsCancelled("", "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 233);
					}
					WebVisit webVisit = await Server.Core.Assistance.Initializer.Dal.Visit.LoadDataAsync(item);
					if (webVisit != null)
					{
						await connection.SendPacketAsync(WebVisit.GenerateSaveRequest(new List<WebVisit> { webVisit }), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 241);
				throw;
			}
			catch (Exception ex)
			{
				ret.AddReturnedValue(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 246);
				WebErrorLog.Instance.StartLog(ex, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\ClsVisit.cs", 247);
			}
			return ret;
		}
	}
}
