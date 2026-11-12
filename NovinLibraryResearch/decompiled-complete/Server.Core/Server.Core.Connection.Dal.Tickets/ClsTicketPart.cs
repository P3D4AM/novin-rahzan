using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using PacketParser;
using PacketParser.Assistance;
using PacketParser.BusinessLayer;
using PacketParser.Entities;
using PacketParser.EventArgs;
using PacketParser.LogPacket;
using Server.Core.Assistance;

namespace Server.Core.Connection.Dal.Tickets;

public class ClsTicketPart : IEntity<WebTicketPart>
{
	private class NestedTicketPart
	{
		internal static readonly ClsTicketPart TicketPartInstance;

		static NestedTicketPart()
		{
			TicketPartInstance = new ClsTicketPart();
		}
	}

	public static ClsTicketPart TicketPartInstance => NestedTicketPart.TicketPartInstance;

	private ClsTicketPart()
	{
	}

	public async Task SendLog(string data)
	{
	}

	public Task<List<WebTicketPart>> ApplyAccessAsync(IEnumerable<WebTicketPart> lst, IClsConnectionData connection)
	{
		using (new JaegerMonitor("ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs"))
		{
			try
			{
				if (!connection.IsAccountingFamily)
				{
					return Task.FromResult<List<WebTicketPart>>(null);
				}
				return Task.FromResult(lst?.ToList());
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "ApplyAccessAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 52);
				return Task.FromResult<List<WebTicketPart>>(null);
			}
		}
	}

	public async Task<ReturnedSaveFuncInfo> Sync2Client(ClsConnectionData connection, string ConnectionUniqueId, bool runInLoopOnce = true)
	{
		using (new JaegerMonitor("Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs"))
		{
			ReturnedSaveFuncInfo ret = new ReturnedSaveFuncInfo();
			try
			{
				if (DateTime.Now.Year > 2025 && DateTime.Now.Month > 10)
				{
					WebErrorLog.Instance.StartLog("یادت باشه پیاده کنی.", "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 64);
				}
				return ret;
			}
			catch (OperationCanceledException exception)
			{
				ret.AddReturnedValue(exception, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 98);
			}
			catch (Exception ex)
			{
				WebErrorLog.Instance.StartLog(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 101);
				await SendLog($"\ud83e\udd9e #TicketPart{connection.DepId} \r\n Sync2Client.#DepId_{connection.DepId}.Source:{connection.Source.GetDisplay()}.Exception:{ex.Message}.\ud83e\udd9e");
				ret.AddReturnedValue(ex, "", "Sync2Client", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 103);
			}
			finally
			{
				Task.Run(() => ClsSync.SendSyncDoneResponseAsync(connection?.cancellationToken ?? default(CancellationToken), connection));
			}
			return ret;
		}
	}

	private async Task InsertDefaultParts(ClsConnectionData connection)
	{
	}

	internal async Task TicketPartInstance_OnSaveRequest(object sender, ConnectionEventArgs<WebTicketPart> e)
	{
		using (new JaegerMonitor("TicketPartInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs"))
		{
			int line = 0;
			try
			{
				line = 1;
				ExpiredPacketLogger.MarkPacketAsProcced(PacketType.TicketPart, 9, isReceivedPacket: true);
				if (e?.ConnectionData == null || e.ConnectionData.Count <= 0 || e.Connection == null || e?.Connection?.DepId <= 0)
				{
					return;
				}
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId);
				line = 5;
				new List<SaveResponse>();
				await SendLog($"\ud83e\udda8 #TicketPart{e.Connection.DepId} \r\n OnSaveRequest.#DepId_{e.Connection.DepId}.Source:{e.Connection.Source.GetDisplay()}.Data Count:{e.ConnectionData.Count} \ud83e\udda8");
				line = 10;
				List<SaveResponse> list = await Server.Core.Assistance.Initializer.Dal.TicketPart.SaveAsync(e.Connection.DepId, e.Connection.HashId, e.ConnectionData, e.Connection.Source, ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId).cancellationToken);
				foreach (SaveResponse item in list)
				{
					item.PartitionId = e.PartitionId;
				}
				line = 15;
				if (InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId))
				{
					line = 16;
					ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebTicketPart.GenerateSaveResponse(list), Priority.High);
					await SendLog($"\ud83d\udc3f #TicketPart{e.Connection.DepId} \r\n OnSaveRequest .#DepId_{e.Connection.DepId}.Source:{e.Connection.Source.GetDisplay()}.Delivery Sent to sender \ud83d\udc3f");
				}
				line = 20;
				List<WebTicketPart> connectionData = e.ConnectionData;
				if (connectionData == null || connectionData.Count <= 0)
				{
					return;
				}
				line = 21;
				List<ClsConnectionData> list2 = ConnectionManager.Instance.GetConnectionsData(e.Connection.DepId)?.Where((ClsConnectionData p) => p?.HashId != e.Connection.HashId && p.Source != ENSource.WebsiteWordPress).ToList();
				line = 230;
				if (list2 == null)
				{
					return;
				}
				foreach (ClsConnectionData conn in list2)
				{
					if (conn == null || conn.IsDisposed)
					{
						continue;
					}
					line = 232;
					List<WebTicketPart> dataForSend = await ApplyAccessAsync(e?.ConnectionData, conn);
					if (dataForSend?.Any() ?? false)
					{
						line = 233;
						Task.Run(() => conn.SendPacketAsync(WebTicketPart.GenerateReadResponse(dataForSend, 0, isSync: false), Priority.Medium));
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, $"Line:{line}", "TicketPartInstance_OnSaveRequest", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 191);
				List<SaveResponse> ret = new List<SaveResponse>
				{
					new SaveResponse
					{
						SaveResultStatus = false
					}
				};
				await SendLog($"\ud83d\udc19 #TicketPart{e.Connection.DepId} \r\n OnSaveRequest Exception.#DepId_{e.Connection.DepId}.message:{ex2.Message} \ud83d\udc19");
				InstanceOnSyncManagement.PacketManager(e.Part, e.TotalParts, e.PacketReferenceId, Result: false);
				ConnectionManager.Instance.GetConnection(e.ConnectionUniqueId)?.SendPacketAsync(WebTicketPart.GenerateSaveResponse(ret), Priority.High);
			}
		}
	}

	public async Task SendNotDeliveredAsync(ClsConnectionData connection, string ConnectionUniqueId)
	{
		using (new JaegerMonitor("SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs"))
		{
			_ = 2;
			try
			{
				List<Guid> list = await Server.Core.Assistance.Initializer.Dal.DalEntitiesDeliveryStatus.GetAllNotSentAsync(connection.DepId, EntityPart.TicketPart, connection.cancellationToken);
				if (connection.cancellationToken.IsCancellationRequested || list == null || (list != null && list.Count <= 0))
				{
					return;
				}
				foreach (Guid item in list)
				{
					if (connection.cancellationToken.IsCancellationRequested)
					{
						break;
					}
					WebTicketPart webTicketPart = await Server.Core.Assistance.Initializer.Dal.TicketPart.LoadDataAsync(item);
					if (webTicketPart != null)
					{
						await connection.SendPacketAsync(WebTicketPart.GenerateSaveRequest(new List<WebTicketPart> { webTicketPart }, 0), Priority.Medium);
					}
				}
			}
			catch (OperationCanceledException)
			{
			}
			catch (Exception ex2)
			{
				WebErrorLog.Instance.StartLog(ex2, "", "SendNotDeliveredAsync", "D:\\Projects\\NPACC\\WebService_Team\\WebService\\Server.Core\\Connection\\Dal\\Tickets\\ClsTicketPart.cs", 267);
			}
		}
	}
}
